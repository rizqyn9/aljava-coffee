using UnityEngine;
using UnityEngine.SceneManagement;
using Game;

public class GameManager : Singleton<GameManager>
{
    [Header("Properties")]
    public GameObject resourcePrefab;
    public string gameScene;
    public string homeScene;
    public SaveData saveData;

    [Header("Debug")]
    public ResourceManager resourceManager;

    private void Start()
    {
        SceneManager.sceneLoaded += handle;
        saveData.loadData();   
    }
    public LevelBase levelBase;
    public void loadLevel(int _level)
    {
        SceneManager.LoadScene(gameScene);
        levelBase = ResourceManager.ListLevels().Find(val => val.level == _level);
    }

    [ContextMenu("Test")]
    public void testLoad()
    {
        SceneManager.LoadScene(homeScene);
        SceneManager.LoadScene(gameScene);
    }

    private void handle(Scene _scene, LoadSceneMode _loadMode)
    {
        if(_scene.name == gameScene)
        {
            Debug.Log("INIT");
            MainController.Instance.Init(levelBase, saveData.userData.levelUserDatas);
        }
    }

    public void backToHome()
    {
        SceneManager.LoadScene(homeScene);
    }
}
