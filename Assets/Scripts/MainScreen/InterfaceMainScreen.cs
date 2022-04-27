using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class InterfaceMainScreen : MonoBehaviour
{
    //Script en escena 0

    public bool canLoadGame;
    public GameObject notGameFound;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //Se carga el booleano para saber si hay partida guardada o no
        //
            PlayerData data = SaveSystem.LoadPlayer();

            canLoadGame = data.canLoadGame;
        //

        Cursor.lockState = CursorLockMode.None;
    }

    public void NewGame() //Función llamada en el botón de Nueva partida
    {
        LoadGameScreen.readyToLoad = false;
        SceneManager.LoadScene(1);
    }
    public void LoadGame() //Función llamada en el botón de Cargar partida
    {
        if (canLoadGame) // Booleano para identificar si hay una partida guardada desde el archivo binario
        {
            LoadGameScreen.readyToLoad = true; //Booleano que se envia al script del player para comunicar que se quiere cagar partida
            SceneManager.LoadScene(2);
        }
        else
        {
            StartCoroutine(WaitForWarning());
        }
    }
    public void ExitButton() //Función llamada en el botón de Salir
    {
        Application.Quit();
    }
    IEnumerator WaitForWarning() //Corrutina para notificar que no hay partida guardada
    {
        notGameFound.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        notGameFound.gameObject.SetActive(false);
    }
}
