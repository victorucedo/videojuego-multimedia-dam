using UnityEngine;

// El láser se enciende y se apaga cada cierto tiempo
public class Laser : MonoBehaviour
{
    public float tiempoCambio;

    float temporizador;
    SpriteRenderer sprite;
    BoxCollider2D caja;

    void Start()
    {
        temporizador = tiempoCambio;
        sprite = GetComponent<SpriteRenderer>();
        caja = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        temporizador -= Time.deltaTime;
        if (temporizador < 0)
        {
            sprite.enabled = !sprite.enabled;
            caja.enabled = sprite.enabled;
            temporizador = tiempoCambio;
        }
    }
}
