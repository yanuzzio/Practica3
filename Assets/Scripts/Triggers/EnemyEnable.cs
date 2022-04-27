using UnityEngine;

public class EnemyEnable : MonoBehaviour
{
    //Script para activar y desactivar los enemigos por zona

     public GameObject enemy1stFloor;
     public GameObject enemy2ndFloor;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            enemy1stFloor.gameObject.SetActive(false);
            enemy2ndFloor.gameObject.SetActive(true);
        }
    }
}
