using UnityEngine;
using UnityEngine.UI;

// Muestra la vida y los puntos en pantalla
public class Marcador : MonoBehaviour
{
    public Text texto;
    public ControlJugador jugador;

    void Update()
    {
        texto.text = "VIDA: " + jugador.vida + "     PUNTOS: " + Puntuacion.puntos;
    }
}
