using UnityEngine;

// EFECTO (extra): Capa del fondo (hija de la cámara). Se desplaza un poco según avanza la cámara:
// las capas lejanas se mueven menos que las cercanas y así se nota la profundidad.
public class Parallax : MonoBehaviour
{
    public Transform camara;
    public float factor;      // 0 = no se mueve respecto a la cámara, 1 = se queda quieta en el mundo
    public float anchoTile;   // ancho de la imagen que se repite

    void LateUpdate()
    {
        // Mathf.Repeat hace que el desplazamiento vuelva a 0 al llegar al ancho de la imagen.
        // Como la imagen se repite, el salto no se nota.
        float x = -Mathf.Repeat(camara.position.x * factor, anchoTile);
        transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
    }
}
