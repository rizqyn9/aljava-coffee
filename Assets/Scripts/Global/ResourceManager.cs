using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ResourceManager : Singleton<ResourceManager>
{
    [Header("Properties")]
    public List<BuyerType> BuyerTypes = new List<BuyerType>();
    public List<MenuType> MenuTypes = new List<MenuType>();
    public MenuType notValidMenu;
    public ResourceData resourceData;

    private void validateResourceData()
    {
        resourceData = new ResourceData()
        {
            buyerTypeCount = BuyerTypes.Count,
            menuTypeCount = MenuTypes.Count
        };
    }

    [ContextMenu("Validate All")]
    public void validateAll()
    {
        Debug.Log("Validate all reources");
        validateBuyer();
        validateResourceData();
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

    /// <summary>
    /// For validate and finding menu result from data list igrendients
    /// </summary>
    /// <param name="_igrendients"></param>
    public bool igrendientsToMenuChecker(List<enumIgrendients> _igrendients, out MenuType resultMenu)
    {
        Debug.Log("igrendientsToMenuChecker on trigger");
        bool res = false;
        resultMenu = notValidMenu;
        for(int i = 0; i < MenuTypes.Count; i++)
        {
            res = MenuTypes[i].recipe.SequenceEqual(_igrendients);
            Debug.Log(res);
            if (res) {
                resultMenu = MenuTypes[i];
                Debug.Log("Found Menu Result");
                break;
            }
        }
        if (!res)
        {
            Debug.Log("Menu not Found");
        }
        return res;
    }
}

