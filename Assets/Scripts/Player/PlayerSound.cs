using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    //Script para los pasos del jugador

    AudioSource audioSource;

    public AudioClip steps;
    public AudioClip stepsCrouch;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip) //Función para reproducir un sonido usada en las funciones de abajo
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
    public void Steps() //Función llamada como un evento en la animación de movimiento
    {
        audioSource.volume = 0.6f;
        PlaySound(steps);
    }
    public void StepsCrouch() //Función llamada como un evento en la animación de movimiento
    {
        audioSource.volume = 0.4f;
        PlaySound(stepsCrouch);
    }
}
