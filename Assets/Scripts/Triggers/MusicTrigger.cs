using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    //Script para controlar los sonidos y la música ambiental desde el escenario ingame
    public bool ambientalMusic;
    public AudioClip terrorSound;
    public AudioClip terrorMusic;
    bool onTrigger = true; //Booleano para que el sonido se reproduzca una sola vez dentro del trigger
    public AudioClip coldBreath;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(!ambientalMusic)
            {
                SoundManager2.instance.PlaySound(terrorSound);
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(ambientalMusic && onTrigger)
            {
                onTrigger = false;

                SoundManager2.instance.ChangeMusicWithFade(terrorMusic);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(ambientalMusic)
            {
                onTrigger = true;
                SoundManager2.instance.ChangeMusicWithFade(coldBreath);
            }
        }
    }
}
