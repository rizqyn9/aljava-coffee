using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class Dev : MonoBehaviour
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
}
