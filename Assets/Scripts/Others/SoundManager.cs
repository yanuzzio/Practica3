using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //Script en escena 2
    public static SoundManager instance;

    public AudioSource effectsSource;
    public AudioSource musicSource;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    public void PlaySound(AudioClip clip) //Función para reproducir un sonido
    {
        effectsSource.clip = clip;
        effectsSource.Play();
    }
    public void PlayMusic(AudioClip clip) //Función para reproducir un sonido en loop
    {
        musicSource.clip = clip;
        musicSource.Play();
    }
}
