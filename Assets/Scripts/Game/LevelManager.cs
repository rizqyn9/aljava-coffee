using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

[System.Serializable]
public struct GlassRegistered
{
    public string glassCode;
    public Glass glass;
}

public class LevelManager : Singleton<LevelManager>
{

    [Header("Debug")]
    public List<GlassRegistered> listGlassRegistered;

    [ContextMenu("test")]
    public void regist<T>(T _t)
    {
        T haha = _t;
        if(_t is CoffeeMaker)
        {
            Debug.Log("is Cofee maker");
        }
    }
}
