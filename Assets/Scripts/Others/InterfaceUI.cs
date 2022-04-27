using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class InterfaceUI : MonoBehaviour
{
    //Script en escena 2
    //Script para manejar el canvas de la escena 2

    [Header ("--References--")]
    public PlayableDirector playableDirector;
    public PlayerHealth playerHealth;
    public PlayerDataLoad playerDataLoad;
    public GameObject interfaceUI;
    public GameObject inventoryUI;
    public GameObject timelineController;
    public GameObject saveCanvas;
    public GameObject settingsCanvas;
    public GameObject canvasImage;
    public GameObject deadImage;
    public GameObject fadeImageStart;

    [Header("--Booleanos--")]
    public bool onCanvas;
    public bool loadEnable;
    bool onIntenvory;
    bool onPause;

    void Update()
    {
        //Condición para cuando se quiere reiniciar a un punto de control desde el menú
        if ((Input.GetKeyUp(KeyCode.U) || Input.GetKeyDown(KeyCode.U)) && loadEnable)
        {
            ReloadGameFromMenu();
        }

        PauseGame();
        Inventory();
    }

    void Inventory() //Función para activar el inventario
    {
        if(Input.GetKey(KeyCode.I) && !onPause)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            inventoryUI.gameObject.SetActive(true);
            onCanvas = true;
            onIntenvory = true;
        }
    }
    void PauseGame() //Función para pausar el game y activar el menú
    {
        if(Input.GetKey(KeyCode.P) && !onIntenvory)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            interfaceUI.gameObject.SetActive(true);
            onCanvas = true;
            onPause = true;
        }
    }
    public void ResumeButton() //Función para reanudar el game desde el menú
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        interfaceUI.gameObject.SetActive(false);
        onCanvas = false;
        onPause = false;
    }
    public void ResumeInventoryButton() //Función para reanudar el game desde el inventario
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inventoryUI.gameObject.SetActive(false);
        onCanvas = false;
        onIntenvory = false;
    }
    public void SaveGame() //Función para cuando se guarda partida llamada en PlayerController
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        saveCanvas.gameObject.SetActive(true);
        onCanvas = true;
    }
    public void ResumenSaveButton() //Función para reanudar partida luego de guardar
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        saveCanvas.gameObject.SetActive(false);
        onCanvas = false;
    }
    public void LoadPointControl() //Función para cargar el punto de control
    {
        Time.timeScale = 1;
        settingsCanvas.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        onCanvas = false;
    }
    public void ReloadGame() //Función para reiniciar el nivel 2 desde cero
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
        playableDirector.enabled = false;
        timelineController.SetActive(true);
        timelineController.GetComponent<EndTimeline>().TimelineEnds();
        onCanvas = false;
    }
    public void ReloadGameFromMenu() //Función para recagar partida desde el menú en "Recomenzar"
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        loadEnable = false;
        ResumeButton();
        canvasImage.gameObject.SetActive(false);
        deadImage.gameObject.SetActive(false);
        playerDataLoad.LoadPlayer();
    }
    public void MainScreen() //Función para ir a la pantalla principal (escena 0)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        onCanvas = false;
    }
    public void ExitGameButton() //Función para salir del juego
    {
        Application.Quit();
    }
    public void bReaload() //Función para activar el booleano cuando se quiere reiniciar a un punto de control
    {
        loadEnable = true;
    }
    public void bReloadNot() //Función para desactivar el booleano cuando se reinició a un punto de control
    {
        loadEnable = false;
    }
}
