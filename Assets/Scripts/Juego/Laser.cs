using UnityEngine;

// El láser se enciende y se apaga cada cierto tiempo.
// El "rayo" es un objeto hijo con el dibujo, la colisión (Peligro) y la luz.
public class Laser : MonoBehaviour
{
    public float tiempoCambio;
    public GameObject rayo;

    float temporizador;

    void Start()
    {
        temporizador = tiempoCambio;
    }

    void Update()
    {
        temporizador -= Time.deltaTime;
        if (temporizador < 0)
        {
            rayo.SetActive(!rayo.activeSelf);
            temporizador = tiempoCambio;
        }
    }
}
