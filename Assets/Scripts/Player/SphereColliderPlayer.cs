using UnityEngine;

public class SphereColliderPlayer : MonoBehaviour
{
    //Script ubicado en el hijo del player para cuando se encuentra cerca del enemigo, este lo detecte

    public bool onRange;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            onRange = true;
            other.gameObject.GetComponent<IAEnemy>().PlayerOnRange();
        }
        else onRange = false;
    }
}
