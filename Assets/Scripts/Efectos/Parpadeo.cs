using UnityEngine;

// EFECTO (extra): Cartel de neón que "falla": cada cierto tiempo (al azar) parpadea un momento
public class Parpadeo : MonoBehaviour
{
    public float tiempoMin;
    public float tiempoMax;
    public float duracionFallo;
    public Behaviour luz;   // la luz 2D del cartel

    SpriteRenderer sprite;
    float temporizador;
    float contadorFallo;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        temporizador = Random.Range(tiempoMin, tiempoMax);
    }

    void Update()
    {
        temporizador -= Time.deltaTime;
        if (temporizador < 0)
        {
            contadorFallo = duracionFallo;
            temporizador = Random.Range(tiempoMin, tiempoMax);
        }

        bool encendido = true;
        if (contadorFallo > 0)
        {
            contadorFallo -= Time.deltaTime;
            encendido = Random.value > 0.5f;   // mientras falla, se enciende y apaga al azar
        }

        sprite.enabled = encendido;
        luz.enabled = encendido;
    }
}
