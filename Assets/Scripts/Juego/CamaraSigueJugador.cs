using UnityEngine;

public class CamaraSigueJugador : MonoBehaviour
{
    public GameObject jugador;
    public float suavizado;

    void LateUpdate()
    {
        // Posición a la que queremos llegar (un poco por encima del jugador)
        Vector3 destino = new Vector3(
            jugador.transform.position.x, jugador.transform.position.y + 2, -10);

        // Lerp: en cada frame la cámara recorre una parte del camino, así se mueve suave
        transform.position = Vector3.Lerp(transform.position, destino, suavizado * Time.deltaTime);
    }
}
