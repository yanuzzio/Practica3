using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueText : MonoBehaviour
{
    //Script del texto que se muestra en la escena 1, activado desde el timeline
    public float delay = 0.1f;
    string fullText;
    string currtentText = "";

    private void Start()
    {
        fullText = "03 - 07 - 1986 Prípiat, Ukraine. \n \n " + "After losing your wife and two daughters in the tragic accident at Chernobyl, you have plunged into loneliness and despair."
             + "You put aside your police job and you lock yourself in your dark reality generating a continuous psychosis. \n \n "
             + "These acts have led you to become an alcoholic and a drug addict. \n \n" + "Guilt and remorse haunt you making your life even more miserable and empty. \n \n"
             + "Until one day ...";

        StartCoroutine(ShowText());
    }
    IEnumerator ShowText() //Corrutina para el Dialogue Text
    {
        //Bucle para escribir el dialogue text letra por letra
        for (int i = 0; i < fullText.Length; i++)
        {
            currtentText = fullText.Substring(0, i + 1);
            this.GetComponent<Text>().text = currtentText;
            yield return new WaitForSeconds(delay);
        }
    }   
}
