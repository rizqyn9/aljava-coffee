using System.IO;
using UnityEngine;
using Game;

public class Dev : Singleton<Dev>
{
    public LevelBase levelBase;
    public bool isDevMode = true;

    [SerializeField] bool useDummyData;
    public InLevelUserData dummyData;

    public void Start()
    {
        if (!isDevMode) return;
        MainController.Instance.Init(levelBase, dummyData);
    }

    [SerializeField] GameState gameState;
    [ContextMenu("Debug Test")]
    public void debugGameState()
    {
        print("On Debug");
        MainController.GameState = gameState;
    }

    [ContextMenu("clear persistant")]
    public void clearPersistant()
    {
        string saveFilePath = Application.dataPath + "/Persistant/aljava.json";

        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.LogWarning("Deleting persistant");
            
        }
        else
        {
            Debug.LogWarning("File not exist");
        }
    }
}
