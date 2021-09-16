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
}
