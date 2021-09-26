using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class Dev : MonoBehaviour
{
    public LevelBase levelBase;
    public bool isDevMode = true;

    public void Start()
    {
        if (!isDevMode) return;
        MainController.Instance.LevelBase = levelBase;
        MainController.Instance.Init();
    }
}
