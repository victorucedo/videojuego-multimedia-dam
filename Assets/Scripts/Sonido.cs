using UnityEngine;

// Reproduce efectos de sonido desde cualquier script.
// Es "static" como Puntuacion: se usa así -> Sonido.Reproducir(miClip);
public class Sonido
{
    public static void Reproducir(AudioClip clip)
    {
        if (clip != null)
        {
            // Se reproduce en la posición de la cámara para que se oiga siempre igual de fuerte
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, 0.6f);
        }
    }
}
