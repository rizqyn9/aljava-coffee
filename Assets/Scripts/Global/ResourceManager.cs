using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if !UNITY_EDITOR
using UnityEditor;
#endif

public class ResourceManager : Singleton<ResourceManager>
{
    [Header("Properties")]
    [SerializeField] List<BuyerType> BuyerTypes = new List<BuyerType>();
    [SerializeField] List<MenuType> MenuTypes = new List<MenuType>();
    [SerializeField] List<MenuClassificationData> MenuClassificationDatas = new List<MenuClassificationData>();
    [SerializeField] List<MachineData> MachineDatas = new List<MachineData>();
    [SerializeField] MenuType notValidMenu;

    #region GlobalAccess
    public static List<BuyerType> ListBuyer() => Instance.BuyerTypes;
    public static List<MenuType> ListMenu() => Instance.MenuTypes;
    public static List<MachineData> ListMachine() => Instance.MachineDatas;
    public static List<MenuClassificationData> ListMenuClass() => Instance.MenuClassificationDatas;

    #endregion

    #region CONTEXT MENU
    [ContextMenu("Validate All")]
    public void validateAll()
    {
        Debug.Log("Validate all reources");
        validateBuyer();
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
    public void validateMenu()
    {
        MenuTypes = Resources.LoadAll<MenuType>("Menu").ToList();
        MenuType _notValid = MenuTypes.Find(val => val.menuListName == MenuListName.NOT_VALID);
        if (_notValid)
        {
            MenuTypes.Remove(_notValid);
            notValidMenu = _notValid;
        }
    }

    [ContextMenu("Validate Menu Classification")]
    public void ValidateMenuClassification() => MenuClassificationDatas = GetTypeData<MenuClassificationData>("MenuClassification");

    [ContextMenu("Validate Machine")]
    public void ValidateMachineData() => MachineDatas = GetTypeData<MachineData>("MachineData");

    #endregion

    public List<T> GetTypeData<T>(string _path) where T : ScriptableObject => Resources.LoadAll<T>(_path).ToList();

    /// <summary>
    /// For validate and finding menu result from data list igrendients
    /// </summary>
    /// <param name="_igrendients"></param>
    public bool igrendientsToMenuChecker(List<MachineIgrendient> _igrendients, out MenuType resultMenu)
    {
        bool res = false;
        resultMenu = notValidMenu;
        for(int i = 0; i < MenuTypes.Count; i++)
        {
            res = MenuTypes[i].Igrendients.SequenceEqual(_igrendients);
            if (res) {
                resultMenu = MenuTypes[i];
                //Debug.Log("Found Menu Result");
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

