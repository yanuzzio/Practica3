[System.Serializable]
public class PlayerData
{
    //Script que contiene los datos que se requieran serializar y deserializar en el formato binario
    public bool dataSave;
    public bool canLoadGame;
    public float[] position;
    public float currentHealth;
    public int pillsAmount;

    public int batteryAmount;

    public bool keyTaked;
    public bool keyTaked2;
    public bool keyTaked3;

    public string weapon1String;
    public string weapon2String;


    public PlayerData(PlayerHealth playerHealth)
    {
        canLoadGame = playerHealth.canLoadGame;

        position = new float[3];
        position[0] = playerHealth.transform.position.x;
        position[1] = playerHealth.transform.position.y;
        position[2] = playerHealth.transform.position.z;

        currentHealth = playerHealth.currentHealth;
        pillsAmount = playerHealth.amountPills;
        batteryAmount = playerHealth.flashlight.batteryAmout;

        keyTaked = playerHealth.playerController.keyTaked;
        keyTaked2 = playerHealth.playerController.keyTaked2;
        keyTaked3 = playerHealth.playerController.keyTaked3;

        if (playerHealth.pickUpWeapon.weapons.Count > 0)
        {
            weapon1String = playerHealth.pickUpWeapon.weapons[0].name;
        }

        if (playerHealth.pickUpWeapon.weapons.Count > 1)
        {
            weapon2String = playerHealth.pickUpWeapon.weapons[1].name;
        }
    }

}
