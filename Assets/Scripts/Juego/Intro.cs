using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Intro con la historia: cada ENTER muestra la siguiente frase. Al acabar, empieza el Nivel 1.
public class Intro : MonoBehaviour
{
    public Text texto;
    public string[] frases;
    public string siguienteEscena;

    int fraseActual;

    void Start()
    {
        texto.text = frases[0];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            fraseActual++;
            if (fraseActual < frases.Length)
            {
                texto.text = frases[fraseActual];
            }
            else
            {
                SceneManager.LoadScene(siguienteEscena);
            }
        }
    }
}
