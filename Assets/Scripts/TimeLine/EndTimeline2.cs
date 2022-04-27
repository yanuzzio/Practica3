using UnityEngine;
using UnityEngine.Playables;

public class EndTimeline2 : MonoBehaviour
{
    //Script para activar y desactivar componentes luego que finaliza el timeline del escenario 2
    public PlayableDirector playableDirector;
    public GameObject soundManager2;
    public GameObject thirdPersonCamera;
    public GameObject player;
    public PlayerController playerController;
    public GameObject enemies;
    public GameObject imageFadeStart;
    public GameObject timeline;

    public void TimelineEndsLoad() //Misma función pero sin el spawn del player (al cagar partida)
    {
        soundManager2.gameObject.SetActive(true);
        thirdPersonCamera.gameObject.SetActive(true);
        enemies.gameObject.SetActive(true);
        imageFadeStart.gameObject.SetActive(true);
       // playerController.enabled = true;
        playableDirector.enabled = false;
        gameObject.SetActive(false);
    }
}
