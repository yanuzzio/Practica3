using UnityEngine;

public class TimeScaleMan : MonoBehaviour
{
    //Script en escenario 2
    private void Awake()
    {
        //Setear el tiempo a 1 para evitar error luego de las cinemáticas
        Time.timeScale = 1;
    }

}
