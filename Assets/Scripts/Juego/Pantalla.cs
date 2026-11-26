using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Para las pantallas de Menú, Victoria y Derrota
public class Pantalla : MonoBehaviour
{
    public Text texto;
    public Text textoRecord;
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

        // El récord se guarda en el ordenador con PlayerPrefs (no se pierde al cerrar el juego)
        int record = PlayerPrefs.GetInt("Record", 0);

        if (mostrarPuntos)
        {
            if (Puntuacion.puntos > record)
            {
                record = Puntuacion.puntos;
                PlayerPrefs.SetInt("Record", record);
            }
            texto.text = mensaje + "\n\nPUNTOS: " + Puntuacion.puntos;
        }
        else
        {
            texto.text = mensaje;
        }

        textoRecord.text = "RÉCORD: " + record;
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
