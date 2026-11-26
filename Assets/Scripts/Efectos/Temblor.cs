using UnityEngine;

// EFECTO (extra): va en la cámara. Cuando el jugador pierde vida, la cámara tiembla un momento.
public class Temblor : MonoBehaviour
{
    public ControlJugador jugador;
    public float fuerza;
    public float duracion;

    int vidaAntes;
    float contador;

    void Start()
    {
        vidaAntes = jugador.vida;
    }

    void LateUpdate()
    {
        if (jugador.vida < vidaAntes)
        {
            contador = duracion;
        }
        vidaAntes = jugador.vida;

        if (contador > 0)
        {
            contador -= Time.deltaTime;
            Vector3 desplazamiento = Random.insideUnitCircle * fuerza;
            transform.position += desplazamiento;
        }
    }
}
