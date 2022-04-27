using UnityEngine;

public class CanLoadGame : MonoBehaviour
{ 
    //Script para recoger el booleano de partida guardada existente
    public bool canLoadGame;

    private void Awake()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        canLoadGame = data.canLoadGame;
    }

}
