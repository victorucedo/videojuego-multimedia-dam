using UnityEngine;

/// <summary>
/// Efecto parallax: las capas del fondo se mueven más despacio que la cámara
/// para dar sensación de profundidad.
/// factor = 1 -> la capa se mueve con la cámara (parece muy lejana)
/// factor = 0 -> la capa está quieta (parece cercana)
/// </summary>
public class Parallax : MonoBehaviour
{
    public Transform camara;
    [Range(0f, 1f)] public float factor = 0.5f;

    private Vector3 posicionInicial;
    private Vector3 camaraInicial;

    void Start()
    {
        if (camara == null && Camera.main != null) camara = Camera.main.transform;
        posicionInicial = transform.position;
        if (camara != null) camaraInicial = camara.position;
    }

    void LateUpdate()
    {
        if (camara == null) return;
        Vector3 delta = camara.position - camaraInicial;
        transform.position = new Vector3(
            posicionInicial.x + delta.x * factor,
            posicionInicial.y + delta.y * factor,
            posicionInicial.z);
    }
}
