using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="BuyerType", menuName ="ScriptableObject/BuyerType")]
public class BuyerType : ScriptableObject
{
    public enumBuyerType enumBuyerType;
    public string buyerName;
    public GameObject buyerPrefab;
    
}
