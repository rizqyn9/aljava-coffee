using System.IO;
using UnityEngine;
using Game;
using UnityEngine.SceneManagement;
using System;

public class Dev : Singleton<Dev>
{
    public LevelBase levelBase;
    public bool isDevMode = true;
    public GameObject gameManager;

    [SerializeField] bool useDummyData;
    public InLevelUserData dummyData;

    public bool isProductionLevel;
    public int levelTarget = 0;

    public void Start()
    {
        if (!isDevMode) return;
        if (SceneManager.GetActiveScene().name == "Game")
        {
            devHandleGame();
        } else
        {
            devHandleStart();
        }
        if (isProductionLevel)
        {
            levelTarget = levelTarget == 0 ? 1 : levelTarget;
            MainController.Instance.Init(ResourceManager.ListLevels().Find(val => val.level == levelTarget), GameManager.Instance.saveData.userData.levelUserDatas);
        }
    }

    private void devHandleStart()
    {
        throw new NotImplementedException();
    }

    private void devHandleGame()
    {
        if (!FindObjectOfType<GameManager>())
        {
            Instantiate(gameManager);
        }
        if (!FindObjectOfType<ResourceManager>())
        {
            Instantiate(GameManager.Instance.resourcePrefab);
        }
        print("SUCCCES");
        if (isProductionLevel) return;
        MainController.Instance.Init(levelBase, dummyData);
        //GameManager.Instance.loadLevel(1);
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
