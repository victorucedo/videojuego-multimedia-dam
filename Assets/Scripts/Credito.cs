using UnityEngine;

public class Credito : MonoBehaviour
{
    public int valor;
    public AudioClip sonido;
    public GameObject particulas;

    Vector3 posInicial;

    void Start()
    {
        posInicial = transform.position;
    }

    void Update()
    {
        // Flota arriba y abajo (seno). Sumamos la x para que no se muevan todos a la vez
        transform.position = posInicial + Vector3.up * Mathf.Sin(Time.time * 3 + posInicial.x) * 0.15f;
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        ControlJugador jugador = otro.GetComponent<ControlJugador>();
        if (jugador != null)
        {
            Puntuacion.puntos += valor;
            Sonido.Reproducir(sonido);
            Instantiate(particulas, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
