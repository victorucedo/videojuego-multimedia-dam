using UnityEngine;

// Punto de control: si el jugador lo toca, al caer por un agujero reaparece aquí
public class Checkpoint : MonoBehaviour
{
    public GameObject luzActiva;
    public AudioClip sonido;

    bool activado;

    void OnTriggerEnter2D(Collider2D otro)
    {
        ControlJugador jugador = otro.GetComponent<ControlJugador>();
        if (jugador != null && !activado)
        {
            activado = true;
            jugador.GuardarCheckpoint(transform.position);
            luzActiva.SetActive(true);
            Sonido.Reproducir(sonido);
        }
    }
}
