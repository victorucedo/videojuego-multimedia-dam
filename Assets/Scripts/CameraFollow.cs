using UnityEngine;

/// <summary>
/// Hace que la cámara siga al jugador de forma suave.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    public Transform objetivo;
    public Vector3 desplazamiento = new Vector3(0f, 2f, -10f);
    [Tooltip("Cuanto más pequeño, más rápido sigue la cámara")]
    public float suavizado = 0.15f;
    [Tooltip("La cámara no bajará de esta altura")]
    public float alturaMinima = -1f;

    private Vector3 velocidadActual;

    void LateUpdate()
    {
        if (objetivo == null) return;

        Vector3 destino = objetivo.position + desplazamiento;
        destino.y = Mathf.Max(destino.y, alturaMinima);
        transform.position = Vector3.SmoothDamp(transform.position, destino, ref velocidadActual, suavizado);
    }
}
