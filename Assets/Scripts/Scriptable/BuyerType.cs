using UnityEngine;

[CreateAssetMenu(fileName ="BuyerType", menuName ="ScriptableObject/BuyerType")]
public class BuyerType : ScriptableObject
{
    public enumBuyerType enumBuyerType;
    public string buyerName;
    public GameObject buyerPrefab;
    public float patienceDuration = 10f;
}
