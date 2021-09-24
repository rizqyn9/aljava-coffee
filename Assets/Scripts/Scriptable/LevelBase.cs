using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelType", menuName = "ScriptableObject/LevelType")]
public class LevelBase : ScriptableObject
{
    public int level;
    public GameMode gameMode;
    public List<menuClassification> MenuClassifications;
    public List<menuListName> MenuTypeUnlock;
    public List<enumBuyerType> BuyerTypeUnlock;

    [Header("Game Mode")]
    public float gameDuration;
    public int minPoint;
    public int minOrder;
    public int minBuyer;
}
