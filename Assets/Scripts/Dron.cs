using UnityEngine;

public class Dron : MonoBehaviour
{
    public float tiempoPatrulla;
    public float velocidad;

    float temporizador;
    int direccion;
    bool muerto;
    SpriteRenderer sprite;
    Rigidbody2D cuerpo;
    Animator anim;

    void Start()
    {
        temporizador = tiempoPatrulla;
        direccion = 1;
        sprite = GetComponent<SpriteRenderer>();
        cuerpo = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Girar();
        ActualizarTemporizador();
    }

    void Girar()
    {
        if (direccion >= 0)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
    }

    void ActualizarTemporizador()
    {
        // Cada cierto tiempo cambia de dirección (patrulla de un lado a otro)
        temporizador -= Time.deltaTime;
        if (temporizador < 0)
        {
            direccion *= -1;
            temporizador = tiempoPatrulla;
        }
    }

    void FixedUpdate()
    {
        if (muerto)
        {
            cuerpo.velocity = Vector2.zero;
        }
        else
        {
            // Vuela en horizontal, sin gravedad
            cuerpo.velocity = new Vector2(direccion * velocidad, 0);
        }
    }

    void OnTriggerStay2D(Collider2D otro)
    {
        ControlJugador jugador = otro.GetComponent<ControlJugador>();
        if (jugador != null)
        {
            if (otro.transform.position.y > transform.position.y + 1.5f)
            {
                // El jugador le cae encima: el dron explota
                jugador.Rebotar();
                Morir();
            }
            else
            {
                // El jugador lo toca de lado o por debajo: recibe daño
                jugador.RecibeDanno(1, transform.position);
            }
        }
    }

    public void Morir()
    {
        if (muerto) return;
        muerto = true;
        Puntuacion.puntos += 50;
        GetComponent<Collider2D>().enabled = false;
        anim.SetBool("Muerto", true);
    }

    // Evento al final de la animación "Explotar"
    public void Destruir()
    {
        Destroy(gameObject);
    }
}
