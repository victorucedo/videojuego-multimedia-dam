using UnityEngine;

// Power-up: durante unos segundos el jugador salta más alto
public class ChipSalto : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D otro)
    {
        ControlJugador jugador = otro.GetComponent<ControlJugador>();
        if (jugador != null)
        {
            jugador.ActivarSuperSalto();
            Destroy(gameObject);
        }
    }
}
