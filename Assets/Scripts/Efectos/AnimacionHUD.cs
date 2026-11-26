using UnityEngine;
using UnityEngine.UI;

// EFECTO (extra): el segmento de vida que se pierde parpadea y el número de créditos "salta" al sumar.
// Se ejecuta en LateUpdate, después del Marcador, para pintar encima de sus colores.
public class AnimacionHUD : MonoBehaviour
{
    public Image[] segmentosVida;
    public Text textoPuntos;
    public ControlJugador jugador;
    public Color colorParpadeo;

    int vidaAntes;
    int puntosAntes;
    float contadorParpadeo;

    void Start()
    {
        vidaAntes = jugador.vida;
        puntosAntes = Puntuacion.puntos;
    }

    void LateUpdate()
    {
        if (jugador.vida < vidaAntes)
        {
            contadorParpadeo = 0.8f;
        }
        vidaAntes = jugador.vida;
        contadorParpadeo -= Time.deltaTime;

        int perdido = jugador.vida;
        if (contadorParpadeo > 0 && perdido >= 0 && perdido < segmentosVida.Length && Mathf.Repeat(Time.time, 0.2f) < 0.1f)
        {
            segmentosVida[perdido].color = colorParpadeo;
        }

        if (Puntuacion.puntos != puntosAntes)
        {
            textoPuntos.transform.localScale = Vector3.one * 1.3f;
            puntosAntes = Puntuacion.puntos;
        }
        textoPuntos.transform.localScale = Vector3.Lerp(textoPuntos.transform.localScale, Vector3.one, 8 * Time.deltaTime);
    }
}
