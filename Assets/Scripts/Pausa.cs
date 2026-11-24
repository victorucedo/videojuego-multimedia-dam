using UnityEngine;

// Menú de pausa: ESC pausa o continúa, M vuelve al menú
public class Pausa : MonoBehaviour
{
    public GameObject panelPausa;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (panelPausa.activeSelf)
            {
                Continuar();
            }
            else
            {
                Pausar();
            }
        }

        if (panelPausa.activeSelf && Input.GetKeyDown(KeyCode.M))
        {
            FindObjectOfType<Fundido>().CambiarEscena("Menu");
        }
    }

    void Pausar()
    {
        panelPausa.SetActive(true);
        Time.timeScale = 0;   // se para el tiempo: física, animaciones y contadores
    }

    void Continuar()
    {
        panelPausa.SetActive(false);
        Time.timeScale = 1;
    }
}
