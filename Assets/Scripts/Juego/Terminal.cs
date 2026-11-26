using UnityEngine;
using UnityEngine.SceneManagement;

// Meta del nivel: al tocarla se "hackea" y, tras una pequeña espera, pasamos a la siguiente escena
public class Terminal : MonoBehaviour
{
    public string siguienteEscena;
    public AudioClip sonido;
    public float tiempoEspera;

    bool hackeado;
    float temporizador;

    void Update()
    {
        // La espera deja que se oiga el sonido del terminal antes de cambiar de escena
        if (hackeado)
        {
            temporizador -= Time.deltaTime;
            if (temporizador < 0)
            {
                SceneManager.LoadScene(siguienteEscena);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        ControlJugador jugador = otro.GetComponent<ControlJugador>();
        if (jugador != null && !hackeado)
        {
            hackeado = true;
            temporizador = tiempoEspera;
            Puntuacion.puntos += 100;
            Sonido.Reproducir(sonido);
        }
    }
}
