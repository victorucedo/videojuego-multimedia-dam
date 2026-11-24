using UnityEngine;
using UnityEngine.UI;

// Intro con la historia: las frases aparecen letra a letra (efecto máquina de escribir).
// ENTER muestra la frase entera o pasa a la siguiente. Al acabar, empieza el Nivel 1.
public class Intro : MonoBehaviour
{
    public Text texto;
    public string[] frases;
    public float letrasPorSegundo;
    public float tiempoEntreFrases;
    public string siguienteEscena;

    int fraseActual;
    float letras;
    float espera;
    bool terminada;

    void Update()
    {
        if (terminada) return;

        string frase = frases[fraseActual];

        // Cuántas letras se ven: va creciendo con el tiempo
        letras += letrasPorSegundo * Time.deltaTime;
        int visibles = Mathf.Min((int)letras, frase.Length);
        texto.text = frase.Substring(0, visibles);

        // Cuando la frase está entera, esperamos un poco y pasamos a la siguiente
        if (visibles == frase.Length)
        {
            espera += Time.deltaTime;
            if (espera > tiempoEntreFrases)
            {
                SiguienteFrase();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (visibles < frase.Length)
            {
                letras = frase.Length;   // completar la frase de golpe
            }
            else
            {
                SiguienteFrase();
            }
        }
    }

    void SiguienteFrase()
    {
        fraseActual++;
        letras = 0;
        espera = 0;

        if (fraseActual >= frases.Length)
        {
            terminada = true;
            FindObjectOfType<Fundido>().CambiarEscena(siguienteEscena);
        }
    }
}
