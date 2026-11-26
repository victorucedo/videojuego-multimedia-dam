using UnityEngine;

// EFECTO (extra): Coche volador que cruza el fondo de la ciudad.
// Es hijo del fondo (que va con la cámara): se mueve en horizontal y al salir por un lado vuelve por el otro.
public class Vehiculo : MonoBehaviour
{
    public float velocidad;
    public float limite;

    void Update()
    {
        transform.localPosition += Vector3.right * velocidad * Time.deltaTime;

        if (velocidad > 0 && transform.localPosition.x > limite)
        {
            transform.localPosition = new Vector3(-limite, transform.localPosition.y, transform.localPosition.z);
        }
        if (velocidad < 0 && transform.localPosition.x < -limite)
        {
            transform.localPosition = new Vector3(limite, transform.localPosition.y, transform.localPosition.z);
        }
    }
}
