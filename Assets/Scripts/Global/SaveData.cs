using UnityEngine;
using Game;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public struct LevelModel
{
    public bool isOpen;
    public int level;
    public int playerInstance;
    public int score;
    public bool isWin;
}

[System.Serializable]
public struct UserData
{
    public string userName;
    public List<LevelModel> listLevels;
}

public class SaveData : MonoBehaviour
{
    [SerializeField] string saveFilePath;
    public UserData userData;
    public LevelModel levelModel;

    private void Awake()
    {
        saveFilePath = Application.dataPath + "/Persistant/aljava.json";
    }

    public void saveUserData()
    {

    }

    public LevelModel grabData()
    {
        return new LevelModel
        {
            level = MainController.Instance.levelBase.level,
            playerInstance = MainController.RulesController.buyerInstanceTotal,
            isWin = MainController.RulesController.isWin
        };
    }

    public void SaveIntoJson()
    {
        try
        {
            string potion = JsonUtility.ToJson(grabData());
            File.WriteAllText(saveFilePath, potion);
            print($"<color=green> Game saved </color>");
        } catch
        {
            print($"<color=red> fail when saving game data </color>");
        }
    }

    [ContextMenu("Simulate Save data")]
    public void simulateSaveData()
    {
        SaveIntoJson();
    }

    [ContextMenu("Get data")]
    public void getData()
    {
        if (File.Exists(saveFilePath))
        {
            userData = JsonUtility.FromJson<UserData>(File.ReadAllText(saveFilePath));
        }
        else
        {
            Debug.LogWarning("File not exist");
        }
    }

    internal void loadUserData()
    {
        getData();
    }
}
