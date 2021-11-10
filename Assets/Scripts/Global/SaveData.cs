using UnityEngine;
using Game;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public struct LevelModel
{
    public bool isOpen;
    public int level;
    public int stars;
    public int playerInstance;
    public int point;
    public bool isWin;
}

[System.Serializable]
public struct UserData
{
    public string userName;
    public int point;
    public InLevelUserData levelUserDatas;
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
            string res = JsonUtility.ToJson(userData);
            File.WriteAllText(saveFilePath, res);
            print($"<color=green> Game saved </color>");
        } catch
        {
            print($"<color=red> fail when saving game data </color>");
        }
    }

    public void loadData()
    {
        if (File.Exists(saveFilePath))
        {
            print($"<color=green> File exist </color>");
            userData = JsonUtility.FromJson<UserData>(File.ReadAllText(saveFilePath));
        }
        else
        {
            Debug.LogWarning("File not exist");
            createTemp();
            SaveIntoJson();
        }
    }

    public void updateLevel(LevelModel _levelModel)
    {
        LevelModel _target = userData.listLevels.Find(val => val.level == _levelModel.level);
        _target = _levelModel;
        if (_levelModel.isWin)
        {
            LevelModel _next = userData.listLevels.Find(val => val.level == _levelModel.level);
            _next.isOpen = true;
        }
        SaveIntoJson();
    }

    public void createTemp()
    {
        userData = new UserData
        {
            userName = "Temp",
            listLevels = new List<LevelModel>()
            {
                new LevelModel
                {
                    isOpen = true,
                    level = 1,
                },
            }
        };
    }
}
