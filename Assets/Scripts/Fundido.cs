using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Imagen negra que tapa la pantalla:
// al empezar la escena se va aclarando, y antes de cambiar de escena se oscurece.
public class Fundido : MonoBehaviour
{
    public Image negro;
    public float velocidad;

    float objetivo = 0;          // 0 = transparente, 1 = negro
    string siguienteEscena = "";

    void Start()
    {
        negro.color = new Color(0, 0, 0, 1);
    }

    void Update()
    {
        // unscaledDeltaTime para que funcione aunque el juego esté en pausa (Time.timeScale = 0)
        Color color = negro.color;
        color.a = Mathf.MoveTowards(color.a, objetivo, velocidad * Time.unscaledDeltaTime);
        negro.color = color;

        if (objetivo == 1 && color.a == 1)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(siguienteEscena);
        }
    }

    public void CambiarEscena(string escena)
    {
        siguienteEscena = escena;
        objetivo = 1;
    }
}
