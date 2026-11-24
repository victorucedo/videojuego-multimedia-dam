using UnityEngine;

// Meta del nivel: al tocarla se "hackea" y pasamos a la siguiente escena
public class Terminal : MonoBehaviour
{
    public string siguienteEscena;
    public AudioClip sonido;
    bool hackeado;

    void OnTriggerEnter2D(Collider2D otro)
    {
        ControlJugador jugador = otro.GetComponent<ControlJugador>();
        if (jugador != null && !hackeado)
        {
            hackeado = true;
            Puntuacion.puntos += 100;
            Sonido.Reproducir(sonido);
            // Con el fundido da tiempo a que suene el terminal antes de cambiar de escena
            FindObjectOfType<Fundido>().CambiarEscena(siguienteEscena);
        }
    }
}
