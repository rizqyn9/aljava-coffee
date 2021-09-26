using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelType", menuName = "ScriptableObject/LevelType")]
public class LevelBase : ScriptableObject
{
    public int level;
    public GameMode gameMode;
    //[Tooltip("")]
    public List<MenuClassification> MenuClassifications;
    public List<menuListName> MenuTypeUnlock;
    public List<enumBuyerType> BuyerTypeUnlock;
    public float delayPerCustomer = 10;

    [Header("Game Mode")]
    public float gameDuration;
    public int minPoint;
    public int minOrder;
    public int minBuyer;
}