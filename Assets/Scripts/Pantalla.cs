using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Para las pantallas de Menú, Victoria y Derrota
public class Pantalla : MonoBehaviour
{
    public Text texto;
    public string mensaje;
    public bool mostrarPuntos;
    public bool reiniciarPuntos;
    public string siguienteEscena;

    void Start()
    {
        if (reiniciarPuntos)
        {
            Puntuacion.puntos = 0;
        }

        if (mostrarPuntos)
        {
            texto.text = mensaje + "\n\nPUNTOS: " + Puntuacion.puntos + "\n\nPulsa ENTER para continuar";
        }
        else
        {
            texto.text = mensaje;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(siguienteEscena);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
