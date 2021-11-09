using UnityEngine;


public class GlobalController : Singleton<GlobalController>
{
    [Header("AnimationController")]
    public float startingAnimLenght = .1f;

    [Header("Properties")]
    public float countingBeforeStart = 3f;
    public float overCookDuration = 1f;
    public int minLevelRender = 9;
}

