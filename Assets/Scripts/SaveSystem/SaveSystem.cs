using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    //Script de guardado con formato binario
    public static void SavePlayer(PlayerHealth playerHealth)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.saveDataPlayer";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(playerHealth);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Saved");
    }
    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.saveDataPlayer";

        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.Log("Not FOUND");
            return null;
        }
    }

    public static void DeleteFile()
    {
        string path = Application.persistentDataPath + "/player.saveDataPlayer";

        File.Delete(path);
    }
}
