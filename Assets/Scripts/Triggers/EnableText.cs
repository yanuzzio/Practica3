using UnityEngine;

public class EnableText : MonoBehaviour
{
    //Script para el texto informativo inicial (escena 2)
    public GameObject inicialText;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inicialText.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inicialText.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
