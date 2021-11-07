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
