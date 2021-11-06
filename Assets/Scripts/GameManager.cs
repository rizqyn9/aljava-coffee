using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("Properties")]
    public string gameScene;
    public SaveData saveData;

    private void Start()
    {
        saveData.loadUserData();   
    }

    public void loadLevel(int _level)
    {
        SceneManager.LoadScene(gameScene);
    }
}
