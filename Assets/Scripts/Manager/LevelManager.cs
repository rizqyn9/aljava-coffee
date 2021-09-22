using System.Collections.Generic;
using UnityEngine;
using Game;

[System.Serializable]
public struct CoffeeProperties
{
    public int delayBeansMachine;
}

public class LevelManager : Singleton<LevelManager>
{
    [Header("Debug")]
    public List<GlassRegistered> listGlassRegistered;
}
