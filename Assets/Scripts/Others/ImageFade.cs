using UnityEngine;
using UnityEngine.SceneManagement;

public class ImageFade : MonoBehaviour
{
    public GameObject interfaceUI;

    public void ImageFadeStartDisable() //Función para permitir acceder a opciones luego que termine el fade del skip del level 1, llamada como evento (gameObject ImageFadeStart)
    {
        interfaceUI.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ImageFadeLastScene() //Función llamada como evento en la animación del gameObjecto ImageFadeEnd, cuando se llega a la puerta final
    {
        SceneManager.LoadScene(3);
    }
}
