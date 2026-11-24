using UnityEngine;

// Power-up: recupera una vida
public class Bateria : MonoBehaviour
{
    public AudioClip sonido;
    public GameObject particulas;

    void OnTriggerEnter2D(Collider2D otro)
    {
        ControlJugador jugador = otro.GetComponent<ControlJugador>();
        if (jugador != null)
        {
            jugador.Curar(1);
            Sonido.Reproducir(sonido);
            Instantiate(particulas, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
