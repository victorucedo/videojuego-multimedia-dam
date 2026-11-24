using UnityEngine;

public class CamaraSigueJugador : MonoBehaviour
{
    public GameObject jugador;
    public float suavizado;
    public float fuerzaTemblor;
    public float duracionTemblor;

    float contadorTemblor;

    void LateUpdate()
    {
        // Posición a la que queremos llegar (un poco por encima del jugador)
        Vector3 destino = new Vector3(
            jugador.transform.position.x, jugador.transform.position.y + 2, -10);

        // Lerp: en cada frame la cámara recorre una parte del camino, así se mueve suave
        transform.position = Vector3.Lerp(transform.position, destino, suavizado * Time.deltaTime);

        // Temblor: mientras dure, movemos la cámara un poco al azar
        if (contadorTemblor > 0)
        {
            contadorTemblor -= Time.deltaTime;
            Vector3 desplazamiento = Random.insideUnitCircle * fuerzaTemblor;
            transform.position += desplazamiento;
        }
    }

    public void Temblar()
    {
        contadorTemblor = duracionTemblor;
    }
}
