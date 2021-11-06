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
        if (!resourceManager)
        {
            resourceManager = Instantiate(resourcePrefab).GetComponent<ResourceManager>();
        }
        saveData.loadUserData();   
    }

    public void loadLevel(int _level)
    {
        SceneManager.LoadScene(gameScene);
    }
}
