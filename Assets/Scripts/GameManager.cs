using UnityEngine;
using UnityEngine.SceneManagement;

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
        saveData.loadData();   
    }

    public void loadLevel(int _level)
    {
        SceneManager.LoadScene(gameScene);
    }

    public void backToHome()
    {
        SceneManager.LoadScene(homeScene);
    }
}
