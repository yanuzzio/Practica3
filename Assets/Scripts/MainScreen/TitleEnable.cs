using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleEnable : MonoBehaviour
{
    //Script en escena 0
    public FlickeringLight flickeringLight;
    public Light lightMainScreen;
    public GameObject titleText;
    public GameObject titleTextSkip;

    void Update()
    {
        //Condición para que el título de la pantalla principal tenga el mismo comportamiento que el pointLight
        if (!flickeringLight.lightEnable)
        {
            titleText.gameObject.SetActive(true);
            titleText.gameObject.SetActive(true);
        }
        else
        {
            titleText.gameObject.SetActive(false);
            titleText.gameObject.SetActive(false);
        }
    }
}
