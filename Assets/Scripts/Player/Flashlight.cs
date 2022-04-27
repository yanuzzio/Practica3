using UnityEngine;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    //Script en la linterna

    [Header ("--Stats--")]
    public float batteryLifeSeconds;
    public float warningInSeconds;
    public int batteryAmout;
    public float batterySetTimer;
    public bool canFlashlight = true;
    bool isActive;
    bool bwarning;
    Light light;

    [Header ("--References--")]
    public AudioClip flashlightOn;
    public AudioClip flashlightOff;
    public AudioClip rechargeSound;
    public Text batteryAmountText;
    public Image lowBattery;
    public PlayerHealth playerHealth;

    void Start()
    {
        light = GetComponent<Light>(); 
    }

    void Update()
    {
        //Condición para el uso de la linterna
        if(!playerHealth.bDeath)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                isActive = !isActive;

                if(isActive) SoundManager.instance.PlaySound(flashlightOn);
                else SoundManager.instance.PlaySound(flashlightOff);
            }

            if(isActive && canFlashlight)
            {
                light.enabled = true;
                bwarning = true;
            }
            else
            {
                light.enabled = false;
                bwarning = false;
            }

            //Condición para cuando se está quedando sin batería
            if (batterySetTimer >= warningInSeconds && bwarning)
            {
                lowBattery.gameObject.SetActive(true);
            }
            else lowBattery.gameObject.SetActive(false);

            TimerBattery();
            Recharge();
        }
    }
    void TimerBattery() //Fcunión para el timer de la batería
    {
        if(light.enabled)
        {
            batterySetTimer += Time.deltaTime;

            if (batterySetTimer >= batteryLifeSeconds)
            {
                batterySetTimer = 0f;
                canFlashlight = false;
            }
            else canFlashlight = true;
        }
    }
    public void Recharge() //Función para recagar la batería y reiniciar timer
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(batteryAmout > 0)
            {
                SoundManager.instance.PlaySound(rechargeSound);

                batteryAmout--;
                canFlashlight = true;
                batterySetTimer = 0f;
                isActive = false;
            }
            else if (batteryAmout <= 0)
            {
                batteryAmout = 0;
                Debug.Log("No tienes más baterias");
            }
        }

        batteryAmountText.text = "x " + batteryAmout;
    }
    public void AddBattery() //Función para añadir una batería al recogerla, llamada en PlayerController
    {
        batteryAmout++;
    }
}
