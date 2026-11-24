using UnityEngine;

public class Credito : MonoBehaviour
{
    public int valor;
    public AudioClip sonido;

    void OnTriggerEnter2D(Collider2D otro)
    {
        ControlJugador jugador = otro.GetComponent<ControlJugador>();
        if (jugador != null)
        {
            Puntuacion.puntos += valor;
            Sonido.Reproducir(sonido);
            Destroy(gameObject);
        }
    }
}
