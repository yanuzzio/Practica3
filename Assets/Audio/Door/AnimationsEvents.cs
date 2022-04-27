using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsEvents : MonoBehaviour
{
    public AudioClip[] doorOpen;
    public AudioClip doorClose;

    public void DoorClose()
    {
        SoundManager.instance.PlaySound(doorClose);
    }

    public void DoorOpen()
    {
        SoundManager.instance.PlaySound(doorOpen[Random.Range(0, doorOpen.Length)]);
    }
}
