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

    int vidaAntes;
    int puntosAntes;
    float contadorParpadeo;

    void Start()
    {
        vidaAntes = jugador.vida;
        puntosAntes = Puntuacion.puntos;
    }

    void Update()
    {
        // Si ha perdido vida, el segmento perdido parpadea un momento
        if (jugador.vida < vidaAntes)
        {
            contadorParpadeo = 0.8f;
        }
        vidaAntes = jugador.vida;
        contadorParpadeo -= Time.deltaTime;

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
            else if (i == jugador.vida && contadorParpadeo > 0 && Mathf.Repeat(Time.time, 0.2f) < 0.1f)
            {
                segmentosVida[i].color = colorPeligro;       // el que se acaba de perder parpadea
            }
            else
            {
                segmentosVida[i].color = colorVacio;
            }
        }

        // Si suben los puntos, el texto "salta" (se agranda) y vuelve poco a poco a su tamaño
        if (Puntuacion.puntos != puntosAntes)
        {
            textoPuntos.transform.localScale = Vector3.one * 1.3f;
            puntosAntes = Puntuacion.puntos;
        }
        textoPuntos.transform.localScale = Vector3.Lerp(textoPuntos.transform.localScale, Vector3.one, 8 * Time.deltaTime);

        textoPuntos.text = "CRÉDITOS  " + Puntuacion.puntos;
    }
}
