using UnityEngine;

public class SoundManager2 : MonoBehaviour
{
    //Script en escena 2
    public static SoundManager2 instance;

    public AudioSource effectsSource;
    public AudioSource musicSource;

    AudioClip newMusic;
    Animator animator;


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

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void PlaySound(AudioClip clip) //Función para reproducir un sonido
    {
        effectsSource.loop = false;
        effectsSource.clip = clip;
        effectsSource.Play();
    }
    public void PlayMusic(AudioClip clip) //Función para reproducir un sonido en loop
    {
        musicSource.clip = clip;
        musicSource.Play();
    }
    public void ChangeMusicWithFade(AudioClip terrorMusic) //Función para cambiar de sonido de forma suave
    {
        newMusic = terrorMusic;

        animator.SetTrigger("Change");
    }
    public void ChangeToTheNewMusic() //Evento llamado en la animación de Change (SoundManager2 animator)
    {
        musicSource.clip = newMusic;
        musicSource.Play();
    }
}
