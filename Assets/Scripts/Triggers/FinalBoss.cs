using System.Collections;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    //Script del final boss, gameObject FinalTrigger
    public IAEnemy iAEnemy; 
    public Transform finalBoss;
    public GameObject finalBossGameObject;
    public Transform lastKey;
    public float secondsToWait;
    public BoxCollider boxCollider;

    void Update()
    {
        if (iAEnemy.gameObject == null) return;

        if(iAEnemy.health <= 0)
        {
            StartCoroutine(WaitForEnemyDeath());
        }
    }

    IEnumerator WaitForEnemyDeath() //Corrutina que se activa cuando el jefe final muere
    {
        lastKey.position = new Vector3(finalBoss.position.x, finalBoss.position.y + 3, finalBoss.position.z);

        yield return new WaitForSeconds(secondsToWait);

        finalBoss.transform.gameObject.GetComponent<Animator>().SetTrigger("LastDeath");
        lastKey.transform.gameObject.GetComponent<Animator>().SetTrigger("Enable");
        this.gameObject.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            boxCollider.isTrigger = false;
            finalBossGameObject.gameObject.SetActive(true);
        }
    }
}
