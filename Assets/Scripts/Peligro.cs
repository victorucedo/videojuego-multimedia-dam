using UnityEngine;

// Para charcos electrificados y láseres: hacen daño mientras los tocas
public class Peligro : MonoBehaviour
{
    public int danno;

    void OnTriggerStay2D(Collider2D otro)
    {
        ControlJugador jugador = otro.GetComponent<ControlJugador>();
        if (jugador != null)
        {
            jugador.RecibeDanno(danno, transform.position);
        }
    }
}
