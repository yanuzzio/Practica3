using UnityEngine;

public class EnemyDisable : MonoBehaviour
{
    //Script para activar y desactivar los enemigos por zona, gameObject EnemyDisable
    public GameObject enemy1stFloor;
    public GameObject enemy2ndFloor;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            enemy1stFloor.gameObject.SetActive(true);
            enemy2ndFloor.gameObject.SetActive(false);
        }
    }
}
