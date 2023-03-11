using UnityEngine;

public class MusicTogler : MonoBehaviour
{
    public AudioSource musicSource;

    public void ToggleMusic()
    {
        if (musicSource.isPlaying)
            musicSource.Pause();
        else
            musicSource.Play();
    }
}
