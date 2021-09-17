using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ResourceManager : Singleton<ResourceManager>
{
    public List<BuyerType> BuyerTypes = new List<BuyerType>();
    public List<MenuType> MenuTypes = new List<MenuType>();

    [ContextMenu("Validate All")]
    public void validateAll()
    {
        Debug.Log("Validate all reources");
        validateBuyer();
    }

    [ContextMenu("Validate Buyer")]
    public void validateBuyer()
    {
        Debug.Log("Validating Buyer");
        BuyerTypes = Resources.LoadAll<BuyerType>("Buyer").ToList();
        foreach(BuyerType _buyerType in BuyerTypes)
        {
            if(BuyerTypes.FindAll(val => val.enumBuyerType == _buyerType.enumBuyerType).Count > 1)
            {
                Debug.LogError("Duplicated");
                break;
            }
        }

        Debug.Log($"Validated Buyers");
    }

    [ContextMenu("Validate Menu")]
    public void validateMenu()
    {
        MenuTypes = Resources.LoadAll<MenuType>("Menu").ToList();
    }
}

