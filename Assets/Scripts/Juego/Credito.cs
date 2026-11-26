using UnityEngine;

public class Credito : MonoBehaviour
{
    public int valor;
    public AudioClip sonido;
    public GameObject particulas;

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
