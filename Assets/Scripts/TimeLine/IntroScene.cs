using UnityEngine;
using UnityEngine.SceneManagement;


public class IntroScene : MonoBehaviour
{
    //Script en escena 1, gameObject Interface
    private void Start()
    {
        SceneManager.LoadScene(2);
    }
}
