using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using UnityEditor;
#if !UNITY_EDITOR
#endif

public class ResourceManager : Singleton<ResourceManager>
{
    [Header("Properties")]
    public List<BuyerType> BuyerTypes = new List<BuyerType>();
    public List<MenuType> MenuTypes = new List<MenuType>();
    public List<MenuClassificationData> MenuClassificationDatas = new List<MenuClassificationData>();
    public List<MachineData> MachineDatas = new List<MachineData>();
    public MenuType notValidMenu;
    public ResourceData resourceData;

    #region CONTEXT MENU
    [ContextMenu("Validate All")]
    public void validateAll()
    {
        Debug.Log("Validate all reources");
        validateBuyer();
        validateResourceData();
        ValidateMenuClassification();
        ValidateMachineData();
    }

    [ContextMenu("Validate Buyer")]
    public void validateBuyer()
    {
        Debug.Log("Validating Buyer");
        BuyerTypes = Resources.LoadAll<BuyerType>("Buyer").ToList();
        foreach (BuyerType _buyerType in BuyerTypes)
        {
            if (BuyerTypes.FindAll(val => val.enumBuyerType == _buyerType.enumBuyerType).Count > 1)
            {
                Debug.LogError("Duplicated");
                break;
            }
        }

        Debug.Log($"Validated Buyers");
    }

    [ContextMenu("Validate Menu")]
    public void validateMenu() => MenuTypes = Resources.LoadAll<MenuType>("Menu").ToList();

    [ContextMenu("Validate Menu Classification")]
    public void ValidateMenuClassification() => MenuClassificationDatas = GetTypeData<MenuClassificationData>("MenuClassification");

    [ContextMenu("Validate Machine")]
    public void ValidateMachineData() => MachineDatas = GetTypeData<MachineData>("MachineData");

    #endregion

    private void validateResourceData()
    {
        resourceData = new ResourceData()
        {
            buyerTypeCount = BuyerTypes.Count,
            menuTypeCount = MenuTypes.Count
        };
    }



    public List<T> GetTypeData<T>(string _path) where T : ScriptableObject => Resources.LoadAll<T>(_path).ToList();

    

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

