using UnityEngine;

// Power-up: durante unos segundos el jugador salta más alto
public class ChipSalto : MonoBehaviour
{
    public AudioClip sonido;
    public GameObject particulas;

    void OnTriggerEnter2D(Collider2D otro)
    {
        ControlJugador jugador = otro.GetComponent<ControlJugador>();
        if (jugador != null)
        {
            jugador.ActivarSuperSalto();
            Sonido.Reproducir(sonido);
            Instantiate(particulas, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
