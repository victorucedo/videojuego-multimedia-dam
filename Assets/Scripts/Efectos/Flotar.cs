using UnityEngine;

// EFECTO (extra): el objeto sube y baja suavemente (se usa en los créditos).
public class Flotar : MonoBehaviour
{
    public float altura;
    public float velocidad;

    Vector3 posInicial;

    void Start()
    {
        posInicial = transform.position;
    }

    void Update()
    {
        transform.position = posInicial + Vector3.up * Mathf.Sin(Time.time * velocidad + posInicial.x) * altura;
    }
}
