using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("Properties")]
    public GameObject resourcePrefab;
    public string gameScene;
    public SaveData saveData;

    [Header("Debug")]
    public ResourceManager resourceManager;

    private void Start()
    {
        saveData.loadUserData();   
    }

    public void loadLevel(int _level)
    {
        SceneManager.LoadScene(gameScene);
    }
}
