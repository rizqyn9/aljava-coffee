using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public SaveData saveData;

    private void Start()
    {
        saveData.loadUserData();   
    }
}
