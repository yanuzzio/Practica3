using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataLoad : MonoBehaviour
{
    public PlayerController playerController;
    public PlayerHealth playerHealth;
    Animator playerHealthAnimator;
    public Flashlight flashlight;
    public PickUpWeapon pickUpWeapon;
    public EndTimeline2 endTimeline2;

    private void Awake()
    {
        /*//Condición para cuando se quiere cargar partida
        if (LoadGameScreen.readyToLoad)
        {
            playerDataLoad.enabled = true;
            LoadGameScreen.readyToLoad = false;
            Time.timeScale = 1;
            endTimeline2.TimelineEndsLoad(); //Llamada a la función para activar los gamObject en escena desactivados por el timeline
        }
        else Debug.Log("Game not found");*/
    }
    void Start()
    {
        playerHealthAnimator = playerHealth.GetComponent<Animator>();

        PlayerData data = SaveSystem.LoadPlayer();

        if (playerHealth.bDeath)
        {
            flashlight.batterySetTimer = 0f;
            playerHealth.setTimerPills = 0;
            playerHealthAnimator.SetTrigger("BackToLife");
            playerHealth.bDeath = false;
        }

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];

        transform.position = position;

        playerHealth.currentHealth = data.currentHealth;
        playerHealth.amountPills = data.pillsAmount;
        flashlight.batteryAmout = data.batteryAmount;
        playerController.keyTaked = data.keyTaked;
        playerController.keyTaked2 = data.keyTaked2;
        playerController.keyTaked3 = data.keyTaked3;


        if (data.weapon1String != null)
        {
            pickUpWeapon.AddWeapon(GameObject.Find(data.weapon1String));
        }

        if (data.weapon2String != null)
        {
            pickUpWeapon.AddWeapon(GameObject.Find(data.weapon2String));
        }

        Transform items = GameObject.Find("Items").transform;
        foreach (Transform _items in items)
        {
            _items.gameObject.SetActive(true);
        }

        Transform triggers = GameObject.Find("Sounds").transform;
        foreach (Transform _triggers in triggers)
        {
            _triggers.gameObject.SetActive(true);
        }
    }

    public void LoadPlayer() //Función de carga de datos
    {
        PlayerData data = SaveSystem.LoadPlayer();

        if (playerHealth.bDeath)
        {
            flashlight.batterySetTimer = 0f;
            playerHealth.setTimerPills = 0;
            playerHealthAnimator.SetTrigger("BackToLife");
            playerHealth.bDeath = false;
        }

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];

        transform.position = position;

        playerHealth.currentHealth = data.currentHealth;
        playerHealth.amountPills = data.pillsAmount;
        flashlight.batteryAmout = data.batteryAmount;
        playerController.keyTaked = data.keyTaked;
        playerController.keyTaked2 = data.keyTaked2;
        playerController.keyTaked3 = data.keyTaked3;


        if (data.weapon1String != null)
        {
            pickUpWeapon.AddWeapon(GameObject.Find(data.weapon1String));
        }

        if (data.weapon2String != null)
        {
            pickUpWeapon.AddWeapon(GameObject.Find(data.weapon2String));
        }

        Transform items = GameObject.Find("Items").transform;
        foreach (Transform _items in items)
        {
            _items.gameObject.SetActive(true);
        }

        Transform triggers = GameObject.Find("Sounds").transform;
        foreach (Transform _triggers in triggers)
        {
            _triggers.gameObject.SetActive(true);
        }

    }
}
