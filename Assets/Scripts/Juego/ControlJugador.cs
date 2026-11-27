using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlJugador : MonoBehaviour
{
    public float velocidad;
    public float fuerzaSalto;
    public float distCaja;
    public Vector2 tamannoCaja;
    public LayerMask capaSuelo;

    public float distAtaque;
    public Vector2 tamannoAtaque;
    public LayerMask capaEnemigo;
    public GameObject fogonazo;
    public GameObject polvo;

    public int vidaMax;
    public int vida;
    public float tiempoInvencible;
    public float tiempoAturdido;
    public float duracionSuperSalto;
    public float alturaVacio;

    public AudioClip sonidoSalto;
    public AudioClip sonidoDisparo;
    public AudioClip sonidoDanno;

    Vector3 movimiento;
    Rigidbody2D cuerpo;
    SpriteRenderer sprite;
    Animator anim;
    bool saltando;
    bool atacando;
    Vector2 knockBack;
    Vector3 puntoReaparicion;
    float contadorInvencible;
    float contadorAturdido;
    float contadorSuperSalto;

    void Start()
    {
        movimiento = new Vector3();
        knockBack = Vector2.zero;
        vida = vidaMax;
        puntoReaparicion = transform.position;
        cuerpo = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        fogonazo.SetActive(false);
    }

    void Update()
    {
        LeerMovimiento();
        LeerSalto();
        LeerAtaque();
        Girar();
        ActualizarAnim();
        ActualizarContadores();
        ComprobarVacio();
    }

    void LeerMovimiento()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            // Izquierda
            movimiento = Vector3.left;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            // Derecha
            movimiento = Vector3.right;
        }
        else
        {
            movimiento = new Vector3();
        }
    }

    void LeerSalto()
    {
        if (Input.GetKeyDown(KeyCode.Space) && EstaEnElSuelo())
        {
            saltando = true;
            Sonido.Reproducir(sonidoSalto);
            // Polvo en los pies al saltar
            Instantiate(polvo, transform.position + Vector3.down * distCaja, Quaternion.identity);
        }
    }

    void LeerAtaque()
    {
        if (Input.GetKeyDown(KeyCode.J) && !atacando)
        {
            atacando = true;
        }
    }

    void Girar()
    {
        // Mientras dispara no se gira
        if (atacando) return;

        if (movimiento.x < 0)
        {
            sprite.flipX = true;
        }
        else if (movimiento.x > 0)
        {
            sprite.flipX = false;
        }
    }

    void ActualizarAnim()
    {
        if (movimiento.x == 0)
        {
            anim.SetBool("Moviendo", false);
        }
        else
        {
            anim.SetBool("Moviendo", true);
        }

        if (EstaEnElSuelo())
        {
            anim.SetBool("Saltando", false);
        }
        else
        {
            anim.SetBool("Saltando", true);
        }

        anim.SetBool("Atacando", atacando);

        if (contadorAturdido > 0)
        {
            anim.SetBool("Herido", true);
        }
        else
        {
            anim.SetBool("Herido", false);
        }
    }

    void ActualizarContadores()
    {
        contadorInvencible -= Time.deltaTime;
        contadorAturdido -= Time.deltaTime;
        contadorSuperSalto -= Time.deltaTime;

        // El color nos indica el estado: rojo = acaba de recibir daño, cian = super salto
        if (contadorInvencible > 0)
        {
            sprite.color = Color.red;
        }
        else if (contadorSuperSalto > 0)
        {
            sprite.color = Color.cyan;
        }
        else
        {
            sprite.color = Color.white;
        }
    }

    void ComprobarVacio()
    {
        // Si se cae por un agujero, pierde una vida y vuelve al último checkpoint
        if (transform.position.y < alturaVacio)
        {
            transform.position = puntoReaparicion;
            cuerpo.velocity = Vector2.zero;
            QuitarVida(1);
        }
    }

    void FixedUpdate()
    {
        if (contadorAturdido > 0)
        {
            // Justo después de un golpe no controlamos al personaje: así se nota el empujón
        }
        else if (atacando && EstaEnElSuelo())
        {
            // Mientras dispara en el suelo se queda quieto (como el Goblin cuando ataca)
            cuerpo.velocity = new Vector2(0, cuerpo.velocity.y);
        }
        else
        {
            cuerpo.velocity = new Vector2(movimiento.x * velocidad, cuerpo.velocity.y);
        }

        if (saltando)
        {
            if (contadorSuperSalto > 0)
            {
                cuerpo.AddForce(Vector2.up * fuerzaSalto * 1.3f);
            }
            else
            {
                cuerpo.AddForce(Vector2.up * fuerzaSalto);
            }
            saltando = false;
        }

        if (knockBack != Vector2.zero)
        {
            cuerpo.velocity = Vector2.zero;
            cuerpo.AddForce(knockBack);
            knockBack = Vector2.zero;
        }
    }

    bool EstaEnElSuelo()
    {
        // Lanzamos una caja hacia abajo: si toca algo de la capa Suelo, estamos en el suelo
        RaycastHit2D rc = Physics2D.BoxCast(
            transform.position, tamannoCaja, 0, Vector2.down, distCaja, capaSuelo);
        if (rc)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    Vector2 DireccionMirada()
    {
        if (sprite.flipX)
        {
            return Vector2.left;
        }
        else
        {
            return Vector2.right;
        }
    }

    // ---- Eventos de la animación "Disparar" (igual que Punnalada en el Goblin) ----

    public void Disparo()
    {
        // Enseñamos el fogonazo delante del arma
        Sonido.Reproducir(sonidoDisparo);
        fogonazo.SetActive(true);
        fogonazo.transform.localPosition = new Vector3(DireccionMirada().x * 1.6f, -0.2f, 0);
        fogonazo.GetComponent<SpriteRenderer>().flipX = sprite.flipX;

        // Lanzamos una caja hacia delante: si toca un enemigo, lo destruye
        RaycastHit2D rc = Physics2D.BoxCast(
            transform.position, tamannoAtaque, 0, DireccionMirada(), distAtaque, capaEnemigo);
        if (rc)
        {
            rc.collider.GetComponent<Dron>().Morir();
        }
    }

    public void FinAtaque()
    {
        fogonazo.SetActive(false);
        atacando = false;
    }

    void OnDrawGizmos()
    {
        // Caja que comprueba el suelo
        Gizmos.DrawWireCube(transform.position + Vector3.down * distCaja, tamannoCaja);
        // Caja del ataque (hacia la derecha)
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + Vector3.right * distAtaque, tamannoAtaque);
        Gizmos.color = Color.white;
    }

    // ---- Métodos que llaman otros scripts (enemigos, peligros, power-ups) ----

    public void RecibeDanno(int cuanto, Vector2 posEnemigo)
    {
        // Si acabamos de recibir un golpe, durante un rato no nos hacen daño
        if (contadorInvencible > 0) return;

        QuitarVida(cuanto);
        Sonido.Reproducir(sonidoDanno);
        contadorAturdido = tiempoAturdido;

        // Si nos golpean mientras disparamos, el ataque se cancela
        atacando = false;
        fogonazo.SetActive(false);

        // Empujón en dirección contraria al enemigo
        Vector2 direccionAtaque = ((Vector2)transform.position) - posEnemigo;
        direccionAtaque.Normalize();
        knockBack = direccionAtaque * 400 + 500 * Vector2.up;
    }

    void QuitarVida(int cuanto)
    {
        vida -= cuanto;
        contadorInvencible = tiempoInvencible;
        Debug.Log("Ouch!! Vida restante: " + vida);
        if (vida <= 0)
        {
            SceneManager.LoadScene("Derrota");
        }
    }

    public void Curar(int cuanto)
    {
        vida += cuanto;
        if (vida > vidaMax)
        {
            vida = vidaMax;
        }
    }

    public void GuardarCheckpoint(Vector3 posicion)
    {
        // A partir de ahora, si se cae, reaparece aquí (un poco más arriba para no quedar dentro del suelo)
        puntoReaparicion = posicion + Vector3.up;
    }

    public void ActivarSuperSalto()
    {
        contadorSuperSalto = duracionSuperSalto;
    }

    public void Rebotar()
    {
        // Al pisar un enemigo damos un pequeño salto
        cuerpo.velocity = new Vector2(cuerpo.velocity.x, 0);
        saltando = true;
    }
}
