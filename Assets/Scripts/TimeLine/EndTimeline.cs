using UnityEngine;
using UnityEngine.Playables;

public class EndTimeline : MonoBehaviour
{
    //Script para activar y desactivar componentes luego que finaliza el timeline del escenario 2
    public PlayableDirector playableDirector;
    public GameObject soundManager2;
    public GameObject thirdPersonCamera;
    public GameObject player;
    public PlayerController playerController;
    public GameObject playerSpawn;
    public GameObject enemies;
    public GameObject imageFadeStart;
    public GameObject timeline;

    public void TimelineEnds() //Función que se llamada al finalizar el timeline
    {
        player.transform.position = playerSpawn.transform.position;
        soundManager2.gameObject.SetActive(true);
        thirdPersonCamera.gameObject.SetActive(true);
        enemies.gameObject.SetActive(true);
        imageFadeStart.gameObject.SetActive(true);
        playableDirector.enabled = false;
        playerController.enabled = true;
        timeline.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
