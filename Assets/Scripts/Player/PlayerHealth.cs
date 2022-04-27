using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class PlayerHealth : MonoBehaviour
{
    [Header ("--Stats--")]
    public float maxHealth;
    public float currentHealth;
    public bool bDeath;
    public bool bDrinking;
    public int amountKit;
    public bool canLoadGame = false;
    Animator animator;

    [Header("--PillsStats--")]
    public float setTimerPills;
    public float cooldownTimerPills;
    public float warningEffects;
    public int amountPills;
    public bool effectsOn;
    public MotionBlur motionBlur;
    bool needForPills = true;

    [Header("--UI--")]
    public Text kitAmountText;
    public Text pillsAmountText;
    public Text healthAmountText;
    public GameObject deadText;

    [Header("--References--")]
    public Flashlight flashlight;
    public PlayerController playerController;
    public PickUpWeapon pickUpWeapon;
    public InterfaceUI interfaceUI;
    public EndTimeline2 endTimeline2;
    public AudioClip hitPlayer;
    public PlayerDataLoad playerDataLoad;

    private void Awake()
    {
        //Condición para cuando se quiere cargar partida
        if (LoadGameScreen.readyToLoad)
        {
            playerDataLoad.enabled = true;
            LoadGameScreen.readyToLoad = false;
            Time.timeScale = 1;
            endTimeline2.TimelineEndsLoad(); //Llamada a la función para activar los gamObject en escena desactivados por el timeline
        }
        else Debug.Log("Game not found");
    }
    void Start()
    {

        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        DrinkPill();
        TimerPills();
        HealthSetting();
        UpDateUI();
    }
    private void LateUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Drink"))
        {
            bDrinking = true;
        }
        else bDrinking = false;
    }
    void UpDateUI() //Función para actualizar el canvas
    {
        kitAmountText.text = "x " + amountKit.ToString();

        pillsAmountText.text = "x " + amountPills.ToString();

        healthAmountText.text = (currentHealth / maxHealth) * 100 + " %";
    }
    public void OnHit(int damage) //Función para el daño hacia el jugador
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            animator.SetTrigger("Die");
            bDeath = true;
            StartCoroutine(WaitForDeath());
        }
        else
        {
            SoundManager.instance.PlaySound(hitPlayer);
            animator.SetTrigger("Hit");
        }
    }
    public void TimerPills() //Función para el timer de las pastillas
    {
        if(needForPills)
        {
            setTimerPills += Time.deltaTime;

            //Condición para cuando no se consume una pastilla al finalizar el tiempo
            if(setTimerPills >= cooldownTimerPills)
            {
                OnHit(100);
                setTimerPills = 0;
                motionBlur.enabled = true;
                needForPills = false;
            }

            if(!bDeath)
            {
                if (setTimerPills >= warningEffects)
                {
                    motionBlur.enabled = true;
                    effectsOn = true;
                }
                else
                {
                    motionBlur.enabled = false;
                    effectsOn = false;
                }
            }
        }
    }
    public void DrinkPill() //Función para consumir la pastilla
    {
        if (Input.GetKeyDown(KeyCode.Z) && amountPills > 0)
        {
            animator.SetTrigger("Drink");
            amountPills--;
            setTimerPills = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Z) && amountPills <= 0) 
        {
            Debug.Log("No tienes más pastillas");
        }
    }
    public void HealthSetting() //Función para aumentar la vida
    {
        if (currentHealth <= 0) currentHealth = 0;
        else if (currentHealth >= maxHealth) currentHealth = maxHealth;

        if (Input.GetKeyDown(KeyCode.C) && amountKit > 0 && currentHealth < maxHealth)
        {
            amountKit--;
            int randomNumber = Random.Range(15, 36);
            currentHealth += randomNumber;
        }
        else if (Input.GetKeyDown(KeyCode.C) && amountKit <= 0) 
        {
            Debug.Log("No tiene más kits");
        }
    }
    public void SaveGame() //Función de guardado
    {
        SaveSystem.SavePlayer(this);
    }
    /*public void LoadPlayer() //Función de carga de datos
    {
        PlayerData data = SaveSystem.LoadPlayer();

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];

        transform.position = position;

        if(bDeath)
        {
            flashlight.batterySetTimer = 0f;
            setTimerPills = 0;
            animator.SetTrigger("BackToLife");
            bDeath = false;
        }


        currentHealth = data.currentHealth;
        amountPills = data.pillsAmount;
        flashlight.batteryAmout = data.batteryAmount;
        playerController.keyTaked = data.keyTaked;
        playerController.keyTaked2 = data.keyTaked2;
        playerController.keyTaked3 = data.keyTaked3;


        if(data.weapon1String != null)
        {
            pickUpWeapon.AddWeapon(GameObject.Find(data.weapon1String));
        }

        if(data.weapon2String != null)
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

        Debug.Log("CARGADO");
    }*/
    public void AddPills(int amount) //Función para cuando se recoge una pildora llamada desde PlayerController
    {
        amountPills += amount;
    }
    public void AddKit() //Función para cuando se recoge un kit llamada desde PlayerController
    {
        amountKit++;
    }
    IEnumerator WaitForDeath() //Corrutina que se activa al morir
    {
        yield return new WaitForSeconds(5f);

        interfaceUI.loadEnable = true;
        deadText.gameObject.SetActive(true);
    }
}
