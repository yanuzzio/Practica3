using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickUpWeapon : MonoBehaviour
{
    [Header ("--References--")]
    public List<GameObject> weapons;
    public GameObject currentWeapon;
    public TextMeshProUGUI collectText;
    public int maxWeapons = 2;
    public Transform hand;
    public Transform dropPoint;
    PlayerController playerController;
    PlayerAttack playerAttack;
    public bool pickUp;

    [Header("--Stats")]
    public int damageAxe;
    public int damageBaseball;
    public int damageWrench;
    public int damageKnife;
    public int damageCrowbar;
    public int damagePipe;
    public int damageRustyKnife;
    public bool knifeEnable; //Booleano para saber si se tiene equipado un Knife


    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    void Update()
    {
        //Condición para saber si está equipada determinada arma
        if(currentWeapon != null)
        {
            if (currentWeapon.name == "Baseball" || currentWeapon.name == "Axe") playerController.heavyWeapon = true;
            else playerController.heavyWeapon = false;
        }

        //Condición para cambiar de arrma
        if(!playerController._bAttacking)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && !weapons[0].activeSelf)
            {
                SelectWeapon(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2) && !weapons[1].activeSelf)
            {
                SelectWeapon(1);
            }
        }

        DropWeapon();
        DamageWeapon();
    }

    void DamageWeapon() //Función para el daño individual de cada armas
    {
        if(currentWeapon != null)
        {
            if (currentWeapon.name == "Axe")
            {
                knifeEnable = false;
                playerAttack.DamageWeapon(damageAxe);
            }
            else if (currentWeapon.name == "Baseball")
            {
                knifeEnable = false;
                playerAttack.DamageWeapon(damageBaseball);
            }   
            else if (currentWeapon.name == "Crowbar")
            {
                knifeEnable = false;
                playerAttack.DamageWeapon(damageCrowbar);
            }
            else if (currentWeapon.name == "Wrench")
            {
                knifeEnable = false;
                playerAttack.DamageWeapon(damageWrench);
            }
            else if (currentWeapon.name == "PipeHomemade")
            {
                knifeEnable = false;
                playerAttack.DamageWeapon(damagePipe);
            }
            else if (currentWeapon.name == "Knife")
            {
                knifeEnable = true;
                playerAttack.DamageWeapon(damageKnife);
            }
            else if (currentWeapon.name == "RustyKnife")
            {
                knifeEnable = true;
                playerAttack.DamageWeapon(damageRustyKnife);
            }
        }
    }
    void SelectWeapon(int index) //Función para la selección del arma
    {
        // Condición para saber si existe el arma
        if (weapons.Count > index && weapons[index] != null)
        {
            // Condición para checkear si hay un arma ya equipada
            if (currentWeapon != null)
            {
                // Se desactiva el arma              
                currentWeapon.gameObject.SetActive(false);
            }

            //Se añade el arma nueva
            currentWeapon = weapons[index];

            // Se muerta el arma nueva
            currentWeapon.SetActive(true);

            playerController.CurrentWeapon(currentWeapon);
        }
    }
    public void DropWeapon() //Función para tirar el arma seleccionada
    {
        if (Input.GetKeyDown(KeyCode.X) && currentWeapon != null)
        {
            currentWeapon.transform.parent = null;

           //currentWeapon.transform.position = dropPoint.position;

            currentWeapon.transform.GetComponent<BoxCollider>().enabled = true;
            currentWeapon.transform.GetComponent<Rigidbody>().isKinematic = false;
            currentWeapon.transform.GetComponent<Rigidbody>().AddForce(Camera.main.transform.up * 5f, ForceMode.Impulse);

            // Se remueve de la lista         
            var weaponInstanceId = currentWeapon.GetInstanceID();
            for (int i = 0; i < weapons.Count; i++)
            {
                if (weapons[i].GetInstanceID() == weaponInstanceId)
                {
                    weapons.RemoveAt(i);
                    break;
                }
            }

            // Se remueve de la mano
            currentWeapon = null;
        }
    }
    public void AddWeapon(GameObject weapon) //Función para cuando se recoge un arma
    {
        if(weapon != null)
        {
            pickUp = true;

            // Se guarda el arma             
            weapons.Add(weapon);

            // Se desactiva al recogerla
            weapon.SetActive(false);
            weapon.GetComponent<Rigidbody>().isKinematic = true;

            // Se posiciona el arma
            weapon.transform.parent = hand;
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.Euler(Vector3.zero);
            weapon.transform.localScale = Vector3.one;

            if (pickUp)
            {
                weapon.GetComponent<BoxCollider>().enabled = false;
                collectText.gameObject.SetActive(false);
                pickUp = false;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        //Condición para recoger el arma
        if (other.gameObject.tag == "Weapon" && weapons.Count < maxWeapons)
        {
            collectText.gameObject.SetActive(true);

            if(Input.GetKey(KeyCode.E))
            {
                AddWeapon(other.gameObject);
                playerController.PickUpItem(other.gameObject);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Weapon")
        {
            collectText.gameObject.SetActive(false);
        }
    }
}

