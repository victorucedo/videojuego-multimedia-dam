using UnityEngine;

/// <summary>
/// Controla el movimiento del personaje principal (Kai):
/// andar/correr con A-D o flechas, saltar con Espacio,
/// y actualiza los parámetros del Animator (Idle, Run, Jump, Hurt).
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 7f;
    public float fuerzaSalto = 14f;

    [Header("Ajustes del salto")]
    [Tooltip("Tiempo extra para poder saltar justo después de salir de una plataforma")]
    public float tiempoCoyote = 0.1f;
    [Tooltip("Si pulsas saltar un poco antes de tocar el suelo, el salto se guarda este tiempo")]
    public float bufferSalto = 0.1f;

    [Header("Detección de suelo")]
    public Vector2 tamanoDeteccionSuelo = new Vector2(0.8f, 0.15f);

    private Rigidbody2D rb;
    private Collider2D col;
    private Animator anim;
    private SpriteRenderer sr;

    private float inputX;
    private bool enSuelo;
    private float contadorCoyote;
    private float contadorBuffer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Entrada del jugador
        inputX = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
            contadorBuffer = bufferSalto;
        else
            contadorBuffer -= Time.deltaTime;

        // Comprobar si estamos en el suelo
        enSuelo = ComprobarSuelo();
        contadorCoyote = enSuelo ? tiempoCoyote : contadorCoyote - Time.deltaTime;

        // Saltar
        if (contadorBuffer > 0f && contadorCoyote > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, fuerzaSalto);
            contadorBuffer = 0f;
            contadorCoyote = 0f;
        }

        // Salto variable: si sueltas el botón pronto, el salto es más corto
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

        // Girar el sprite hacia donde miramos
        if (inputX > 0.01f) sr.flipX = false;
        else if (inputX < -0.01f) sr.flipX = true;

        // Actualizar animaciones
        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(inputX));
            anim.SetBool("Grounded", enSuelo);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(inputX * velocidad, rb.velocity.y);
    }

    private bool ComprobarSuelo()
    {
        Bounds b = col.bounds;
        Vector2 pies = new Vector2(b.center.x, b.min.y);
        Collider2D[] golpes = Physics2D.OverlapBoxAll(pies, tamanoDeteccionSuelo, 0f);
        foreach (Collider2D c in golpes)
        {
            if (c != col && !c.isTrigger) return true;
        }
        return false;
    }

    /// <summary>
    /// Llamar a este método cuando el jugador reciba daño (enemigos, hazards...).
    /// De momento solo reproduce la animación de daño.
    /// </summary>
    public void RecibirGolpe()
    {
        if (anim != null) anim.SetTrigger("Hurt");
    }

    void OnDrawGizmosSelected()
    {
        Collider2D c = GetComponent<Collider2D>();
        if (c == null) return;
        Gizmos.color = Color.green;
        Bounds b = c.bounds;
        Gizmos.DrawWireCube(new Vector3(b.center.x, b.min.y, 0f), tamanoDeteccionSuelo);
    }
}
