using UnityEngine;
using UnityEngine.SceneManagement;

public class LastScene : MonoBehaviour
{
    //Script en escena 3
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void ExitGame() //Función llamada en el botón Salir
    {
        Application.Quit();
    }
    public void MainScreen() //Función llamada en el botón Pantalla principal
    {
        SceneManager.LoadScene(0);
    }
}
