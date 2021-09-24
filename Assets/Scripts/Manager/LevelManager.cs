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
    [SerializeField] LevelBase _levelBase;
    public LevelBase LevelBase {
        get => _levelBase;
        set => _levelBase = value;
    }

    public List<GlassRegistered> listGlassRegistered;

}
