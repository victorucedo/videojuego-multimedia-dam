using UnityEngine;

public class CamaraSigueJugador : MonoBehaviour
{
    public GameObject jugador;
    public float suavizado;

    // Tamaño de la vista (más grande = se ve más escenario)
    public float tamanoVista = 9f;
    // La cámara nunca sube más de esta altura, así siempre se ve el suelo
    public float alturaMaxima = 3f;
    // La cámara nunca baja más de esta altura
    public float alturaMinima = -2f;

    void Start()
    {
        Camera cam = GetComponent<Camera>();
        if (cam != null && cam.orthographic) cam.orthographicSize = tamanoVista;
    }

    void LateUpdate()
    {
        // Un poco por encima del jugador, pero limitada en altura para no perder el suelo
        float y = Mathf.Clamp(jugador.transform.position.y + 2, alturaMinima, alturaMaxima);
        Vector3 destino = new Vector3(jugador.transform.position.x, y, -10);

        // Lerp: en cada frame la cámara recorre una parte del camino, así se mueve suave
        transform.position = Vector3.Lerp(transform.position, destino, suavizado * Time.deltaTime);
    }
}
