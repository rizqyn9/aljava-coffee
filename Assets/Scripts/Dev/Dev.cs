using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dev : MonoBehaviour
{
    public LevelBase levelBase;
    public bool isDevMode = true;

    public void Start()
    {
        if (!isDevMode) return;
        LevelManager.Instance.LevelBase = levelBase;
    }
}
