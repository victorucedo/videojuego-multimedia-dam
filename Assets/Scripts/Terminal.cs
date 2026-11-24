using UnityEngine;
using UnityEngine.SceneManagement;

// Meta del nivel: al tocarla se "hackea" y pasamos a la siguiente escena
public class Terminal : MonoBehaviour
{
    public string siguienteEscena;

    void OnTriggerEnter2D(Collider2D otro)
    {
        ControlJugador jugador = otro.GetComponent<ControlJugador>();
        if (jugador != null)
        {
            Puntuacion.puntos += 100;
            SceneManager.LoadScene(siguienteEscena);
        }
    }
}
