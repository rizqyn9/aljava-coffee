using UnityEngine;
using Game;
using System.IO;

[System.Serializable]
public struct LevelModel
{
    public int level;
    public int playerInstance;
    public int score;
    public bool isWin;
}

public class SaveData : MonoBehaviour
{

    [SerializeField] string saveFilePath;
    public LevelModel levelModel;

    private void Awake()
    {
        saveFilePath = Application.dataPath + "/Persistant/aljava.json";
    }

    public LevelModel grabData()
    {
        return new LevelModel
        {
            level = MainController.Instance.levelBase.level,
            playerInstance = RulesController.Instance.buyerInstanceTotal,
            isWin = false,
        };
    }

    public void SaveIntoJson()
    {
        string potion = JsonUtility.ToJson(grabData());
        File.WriteAllText(saveFilePath, potion);
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
            levelModel = JsonUtility.FromJson<LevelModel>(File.ReadAllText(saveFilePath));
        }
        else
        {
            print("File not exist");
        }

    }


}
