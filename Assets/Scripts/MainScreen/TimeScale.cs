using UnityEngine;
public class TimeScale : MonoBehaviour
{
    //Script en escena 0
    public float timeValue;
    private void Start()
    {
        //Volver el timeScale a 1 para evitar errores con la cinemáticas
        Time.timeScale = 1;
    }
}
