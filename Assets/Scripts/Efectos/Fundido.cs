using UnityEngine;
using UnityEngine.UI;

// EFECTO (extra): al empezar la escena, una imagen negra se va aclarando hasta desaparecer.
public class Fundido : MonoBehaviour
{
    public Image negro;
    public float velocidad;

    void Start()
    {
        negro.color = new Color(0, 0, 0, 1);
    }

    void Update()
    {
        Color color = negro.color;
        color.a = Mathf.MoveTowards(color.a, 0, velocidad * Time.unscaledDeltaTime);
        negro.color = color;
    }
}
