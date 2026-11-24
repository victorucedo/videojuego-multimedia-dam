using UnityEngine;

// Power-up: recupera una vida
public class Bateria : MonoBehaviour
{
    public AudioClip sonido;

    void OnTriggerEnter2D(Collider2D otro)
    {
        ControlJugador jugador = otro.GetComponent<ControlJugador>();
        if (jugador != null)
        {
            jugador.Curar(1);
            Sonido.Reproducir(sonido);
            Destroy(gameObject);
        }
    }
}
