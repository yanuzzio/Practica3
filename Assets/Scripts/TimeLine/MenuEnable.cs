using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuEnable : MonoBehaviour
{ 
    //Ubicación: InterfaceUI
    public GameObject menu;

    public void _MenuEnable() // Función llamada en el FadeSkip (MainScreen) para activar el menú luego de terminar el fade
    {
        menu.gameObject.SetActive(true);
    }
}
