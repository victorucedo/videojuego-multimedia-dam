using UnityEngine;
using UnityEngine.UI;

// HUD: la vida en segmentos (una barra por vida) y los créditos
public class Marcador : MonoBehaviour
{
    public Image[] segmentosVida;
    public Text textoPuntos;
    public ControlJugador jugador;
    public Color colorLleno;
    public Color colorVacio;
    public Color colorPeligro;

    void Update()
    {
        for (int i = 0; i < segmentosVida.Length; i++)
        {
            if (i < jugador.vida)
            {
                if (jugador.vida == 1)
                {
                    segmentosVida[i].color = colorPeligro;   // última vida: en rojo
                }
                else
                {
                    segmentosVida[i].color = colorLleno;
                }
            }
            else
            {
                segmentosVida[i].color = colorVacio;
            }
        }

        textoPuntos.text = "CRÉDITOS  " + Puntuacion.puntos;
    }
}
