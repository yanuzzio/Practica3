using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("--Stats--")]
    public float walkSpeed = 4f;
    public float runSpeed = 6f;
    public float crouchSpeed = 2f;
    public float turnSmoothTime = 0.1f;
    public float speedSmoothTime = 0.1f;
    float turnSmoothVelocity;
    float gravity = 9.8f;
    float currentSpeed = 0f;
    Vector3 direction;
    Vector3 moveDir;
    public int _batteryAmount;

    [Header("--Booleanos--")]
    public bool _bIdle;
    public bool _bWalking;
    public bool _bRunning;
    public bool _bCrounched;
    public bool _bAttacking;
    public bool _bHiting;
    public bool heavyWeapon; //Booleano llamado en PickUpWeapon
    public bool keyTaked;
    public bool keyTaked2;
    public bool keyTaked3;
    bool openDoor;
    bool inputAction;
    bool bcrounched;
    bool brunning;

    [Header("--Detection--")]
    public LayerMask zombieMask;
    public float radiusDetection = 0f;
    public Transform sphereCastSpawn;
    public float radiusSphereCast;
    public int damageToEnemy;

    [Header("--References--")]
    public CharacterController characterController;
    public Flashlight flashlight;
    public InterfaceUI interfaceUI;
    public PickUpWeapon pickUpWeapon;
    public PlayerHealth playerHealth;
    public GameObject imageFadeLastScene;
    Animator animator;

    [Header ("--Cameras--")]
    public Transform cam;
    bool bvirtualCameraOn; //Booleano que se activa al visualizar el mapa

    [Header("--Sound--")]
    public AudioClip doorOpen;
    public AudioClip doorClose;
    public AudioClip boxOpen;

    [Header ("--Text--")]
    public TextMeshProUGUI exitCameraText;
    public TextMeshProUGUI interactText;
    public TextMeshProUGUI collectText;
    public TextMeshProUGUI collectAfterText;
    public TextMeshProUGUI noKeyAvaibleText;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        //Condición para que el jugador no pueda controlar el personaje
        //Cuando se visualiza el mapa, cuando el player está muerto y cuando se está en el menú o en el inventario
        if (bvirtualCameraOn || playerHealth.bDeath || interfaceUI.onCanvas) return;

        SoundDetection();
        OthersMovements();
        Movement();
        EmergencyCursor();

        if (Input.GetKeyDown(KeyCode.Y))
        {
            SaveSystem.DeleteFile();
        }
    }

    private void LateUpdate()
    {
        //Control del comportamiento de las animaciones

        animator.SetBool("Crouch", bcrounched);

        if (Input.GetKey(KeyCode.LeftShift) && direction.magnitude == 1)
        {
            animator.SetFloat("Movement", 2f * direction.magnitude, speedSmoothTime, Time.deltaTime);
            _bIdle = false;
            _bWalking = false;
            _bRunning = true;
        }
        else if (direction.magnitude == 1)
        {
            animator.SetFloat("Movement", direction.magnitude, speedSmoothTime, Time.deltaTime);
             _bIdle = false;
            _bWalking = true;
            _bRunning = false;
        }
        else if (direction.magnitude == 0)
        {
            animator.SetFloat("Movement", direction.magnitude, speedSmoothTime, Time.deltaTime);
            _bIdle = true;
            _bWalking = false;
            _bRunning = false;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Crouch"))
        {
            _bCrounched = true;
        }
        else _bCrounched = false;

        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            _bAttacking = true;
        }
        else _bAttacking = false;

        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Hit"))
        {
            _bHiting = true;
        }
        else _bHiting = false;
    }
    void Movement() //Función para el moviemiento
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontalInput, 0, verticalInput).normalized;

        if (direction.magnitude >= 0.1f && !_bAttacking && !playerHealth.bDrinking && !_bHiting)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Condiciones para las diferentes velocidades dependiendo de su estado
            if (brunning)
            {
                currentSpeed = runSpeed;
            }
            else if (bcrounched)
            {
                currentSpeed = crouchSpeed;
            }
            else if(!brunning && !bcrounched)
            {
                currentSpeed = walkSpeed;
            }

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDir.y = -gravity * 50 * Time.deltaTime;
        }
        else currentSpeed = 0;
        
        //Condición para cuando se lleva el Baseball o el Axe. Booleano cambia en PickUpWeapon
        float actualSpeed = currentSpeed;

        if (heavyWeapon) currentSpeed *= 0.8f;
        else currentSpeed = actualSpeed;

        characterController.Move(moveDir * currentSpeed * Time.deltaTime);
    }
    void OthersMovements() //Función para los condicionantes de otros moviemientos (correr, agacharse y atacar)
    {
        if(!_bAttacking && !playerHealth.bDrinking)
        {
            if (Input.GetKey(KeyCode.LeftControl) )
            {
                bcrounched = true;
            }
            else bcrounched = false;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                brunning = true;
            }
            else brunning = false;

            if(Input.GetMouseButtonDown(0) && pickUpWeapon.currentWeapon != null)
            {
                if (pickUpWeapon.knifeEnable) animator.SetTrigger("Attack2");
                else animator.SetTrigger("Attack");
            }
        }
    }
    void SoundDetection() //Función para generar una esfera que colicione con el enemigo. El radio dependerá del estado del player
    {
        //Valores para el radio que se genera en los diferentes estados cambie respectivamente
        if (_bIdle) radiusDetection = 0;
        else if (_bWalking && !_bCrounched) radiusDetection = 7f;
        else if (_bRunning) radiusDetection = 10f;
        else if (_bCrounched && _bWalking) radiusDetection = 2.5f;

        Collider[] zombie = Physics.OverlapSphere(transform.position, radiusDetection, zombieMask);
        for (int i = 0; i < zombie.Length; i++)
        {
            zombie[i].GetComponent<IAEnemy>().PlayerOnRange();
        }
    }
    public void Interact(TextMeshProUGUI textInScreen, bool active) //Función para activar y desactivar el texto en pantalla
    {
        textInScreen.gameObject.SetActive(active);
    }
    void DoorKey(GameObject _gameObject, bool _keyTaked) //Función para visualizar el texto cuando se recoge una llave llamada desde el triggerStay
    {
        if (Input.GetKey(KeyCode.Q))
        {
            if (_keyTaked)
            {
                Interact(interactText, false);
                _gameObject.transform.GetChild(0).GetComponent<Animator>().SetBool("DoorOpen", true);
            }
            else
            {
                StartCoroutine(WaitForTheKey(noKeyAvaibleText, 3f));
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //Condición para cuandod se entra en rango con el enemigo
        if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<IAEnemy>().PlayerOnRange();
        }

        if (other.gameObject.tag == "Battery" || other.gameObject.tag == "Pills")
        {
            Interact(collectText, true);
        }

    }
    private void OnTriggerStay(Collider other)
    {
        //Condición para interaccionar con las puertas
        if (other.gameObject.tag == "Door")
        {
            if(!openDoor) Interact(interactText, true);

            if(Input.GetKey(KeyCode.Q))
            {
                Interact(interactText, false);
                other.gameObject.transform.GetChild(0).GetComponent<Animator>().SetBool("DoorOpen", true);
                openDoor = true;
            }
        }

        //Condición para recoger la llave1
        if(other.gameObject.tag == "DoorKey")
        {
            Interact(interactText, true);

            DoorKey(other.gameObject, keyTaked);
        }

        //Condición para recoger la llave2
        if (other.gameObject.tag == "DoorKey2")
        {
            Interact(interactText, true);

            DoorKey(other.gameObject, keyTaked2);
        }

        //Condición para recoger la llave final
        if (other.gameObject.tag == "FinalDoor")
        {
            Interact(interactText, true);

            if (Input.GetKey(KeyCode.Q) && keyTaked3)
            {
                bvirtualCameraOn = true;
                StartCoroutine(WaitForFinalScene());
            }
        }

        //Condición para cuando se abre un cajón 
        if (other.gameObject.tag == "Box")
        {
            if (!inputAction) Interact(interactText, true);

            if (Input.GetKey(KeyCode.Q))
            {
                Interact(interactText, false);
                other.gameObject.transform.GetChild(0).GetComponent<Animator>().SetBool("Open", true);
                SoundManager.instance.PlaySound(boxOpen);
                other.gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                inputAction = true;
            }
        }

        //Condición para cuando se activa la interacción con el mapa
        if (other.gameObject.tag == "Camera")
        {
            if(!bvirtualCameraOn) Interact(interactText, true);

            if (Input.GetKey(KeyCode.Q)) //Cambio de cámara
            {
                Interact(interactText, false); //Texto de la UI
                other.gameObject.transform.GetChild(0).gameObject.SetActive(true); //Linterna del mapa on
                gameObject.transform.GetChild(0).gameObject.SetActive(false);  //Linterna del player out
                exitCameraText.gameObject.SetActive(true); //Texto para salir del mapa
                //Time.timeScale = 0;
                bvirtualCameraOn = true;

            }
            else if(bvirtualCameraOn && Input.GetKey(KeyCode.C)) //Vuelta a la cámara del player
            {
                other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                exitCameraText.gameObject.SetActive(false);
                //Time.timeScale = 1;
                bvirtualCameraOn = false;
            }
        }

        //Condición para recoger un item
        if(other.gameObject.layer == 13) //Layer Item
        {
            Interact(collectText, true);

            if(Input.GetKey(KeyCode.E))
            {
                Interact(collectText, false);

                if (other.gameObject.tag == "Battery") //Batería
                {
                    flashlight.GetComponent<Flashlight>().AddBattery();
                    collectAfterText.text = "You have collected a battery";
                }
                else if(other.gameObject.tag == "Pills") //Pastillas
                {
                    int randomPills = Random.Range(0, 4);
                    playerHealth.AddPills(randomPills);

                    if(randomPills == 0)
                    {
                        collectAfterText.text = "You havent found any pills";
                    }
                    else if (randomPills == 1) collectAfterText.text = "You have collected " + randomPills + " pill";
                    else collectAfterText.text = "You have collected " + randomPills + " pills";
                }
                else if (other.gameObject.tag == "Key") //Llave1
                {
                    keyTaked = true;
                    collectAfterText.text = "You have collected a key from the 2nd floor";
                }
                else if (other.gameObject.tag == "Key2") //Llave2
                {
                    keyTaked2 = true;
                    collectAfterText.text = "You have collected a key from the 1st floor";
                }
                else if (other.gameObject.tag == "FinalKey") //Llave final
                {
                    keyTaked3 = true;
                    collectAfterText.text = "You have collected the final key";
                }
                else if (other.gameObject.tag == "Kit") //Kit médico
                {
                    playerHealth.AddKit();
                    collectAfterText.text = "You have collected a medical kit";
                }

                other.gameObject.SetActive(false);
                collectAfterText.gameObject.GetComponent<Animator>().SetTrigger("Enable");
            }
        }

        //Condición para guardar el game
        if(other.gameObject.tag == "SaveGame")
        {
            if(!inputAction) Interact(interactText, true);

            other.gameObject.transform.GetChild(0).transform.gameObject.SetActive(true);

            if(Input.GetKey(KeyCode.Q))
            {
                playerHealth.canLoadGame = true;
                Interact(interactText, false);
                interfaceUI.SaveGame();
                inputAction = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //Condiciones para desactivar el texto y/o animacíon al salir del trigger

        if(other.gameObject.tag == "Door" || other.gameObject.tag == "DoorKey2")
        {
            other.gameObject.transform.GetChild(0).GetComponent<Animator>().SetBool("DoorOpen", false);
            openDoor = false;
            Interact(interactText, false);
        }

        if (other.gameObject.tag == "Box")
        {
            inputAction = false;
            Interact(interactText, false);
        }

        if(other.gameObject.tag == "SaveGame")
        {
            other.gameObject.transform.GetChild(0).transform.gameObject.SetActive(false);
            Interact(interactText, false);
            inputAction = false;
        }

        if (other.gameObject.tag == "Camera" || other.gameObject.tag == "DoorKey" || other.gameObject.tag == "DoorKey2" || other.gameObject.tag == "FinalDoor")
        {
            Interact(interactText, false);
        }

        if(other.gameObject.tag == "Battery" || other.gameObject.tag == "Pills" || other.gameObject.tag == "Key" || other.gameObject.tag == "Key2" || other.gameObject.tag == "FinalKey" || other.gameObject.tag == "Kit")
        {
            Interact(collectText, false);
        }
    }
    public void PickUpItem(GameObject gameObject) //Función para visualizar el texto al recoger un arma, llamada en PickUpWeapon
    {
        collectAfterText.text = "You picked up the weapon: " + gameObject.name;
        collectAfterText.gameObject.GetComponent<Animator>().SetTrigger("Enable");
    }
    public void CurrentWeapon(GameObject gameObject) //Función para visualizar el texto al equipar un arma, llamada en PickUpWeapon
    {
        collectAfterText.text = "You have equipped the weapon: " + gameObject.name;
        collectAfterText.gameObject.GetComponent<Animator>().SetTrigger("Enable");
    }
    public void EmergencyCursor()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    IEnumerator WaitForTheKey(TextMeshProUGUI text, float time) //Corrutina para visualizar el texto al no contener la llave de la correspondiente puerta
    {
        text.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        text.gameObject.SetActive(false);
    }
    IEnumerator WaitForFinalScene() //Corrutina al interactuar con la puerta final luego de derrotar al jefe final
    {
        yield return new WaitForSeconds(3f);

        imageFadeLastScene.gameObject.SetActive(true);
    }
    private void OnDrawGizmos() //Función para visualizarr los radios de detección
    {
        Gizmos.DrawWireSphere(transform.position, radiusDetection);

        Gizmos.DrawWireSphere(sphereCastSpawn.position, radiusSphereCast);
    }
}
