using UnityEngine;
using UnityEngine.UI;

// EFECTO (extra): el texto se ve 0,6 s de cada segundo (para "PULSA ENTER").
public class TextoParpadeante : MonoBehaviour
{
    Text texto;

    void Start()
    {
        texto = GetComponent<Text>();
    }

    void Update()
    {
        texto.enabled = Mathf.Repeat(Time.time, 1) < 0.6f;
    }
}
