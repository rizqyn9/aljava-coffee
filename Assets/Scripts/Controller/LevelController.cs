using System.Collections.Generic;
using UnityEngine;
using Game;

[System.Serializable]
public struct CoffeeProperties
{
    public int delayBeansMachine;
}

[System.Serializable]
public struct ResourceCount
{
    public int MenuCount;
    public int BuyerCount;
    public int MachineCount;
}

/// <summary>
/// Grab all data level
/// <list type="bullet">
/// Buyer Unlock
/// Menu Unlock
/// </list>
/// </summary>
public class LevelController : Singleton<LevelController>, IController
{
    [Header("Debug")]
    [SerializeField] LevelBase _levelBase;
    public List<MenuType> MenuTypes = new List<MenuType>();
    public List<BuyerType> BuyerTypes = new List<BuyerType>();
    public List<MachineData> MachineDatas = new List<MachineData>();
    public List<MenuClassificationData> MenuClassificationDatas = new List<MenuClassificationData>();
    public ResourceCount ResourceCount;

    public LevelBase LevelBase {
        get => _levelBase;
        set { _levelBase = value; setData(); }
    }

    public GameState GameState { get ; set ; }

    private void setData()
    {
        MenuTypes = ResourceManager.Instance.MenuTypes.FindAll(val => _levelBase.MenuTypeUnlock.Contains(val.menuListName));
        BuyerTypes = ResourceManager.Instance.BuyerTypes.FindAll(val => _levelBase.BuyerTypeUnlock.Contains(val.enumBuyerType));
        MenuClassificationDatas = ResourceManager.Instance.MenuClassificationDatas.FindAll(val => _levelBase.MenuClassifications.Contains(val.MenuClassification));

        ResourceCount = new ResourceCount()
        {
            BuyerCount = BuyerTypes.Count,
            MenuCount = MenuTypes.Count
        };

        GetMachineMustSpawn();
    }

    internal void Init()
    {
        MainController.Instance.AddController(this);
    }

    private void GetMachineMustSpawn()
    {
        List<MachineType> MachineTypes = new List<MachineType>();

        foreach(MenuType _menuType in MenuTypes)
        {
            foreach(MachineType _machineType in _menuType.Igrendients)
            {
                if (!MachineTypes.Contains(_machineType))
                {
                    MachineTypes.Add(_machineType);
                    MachineDatas.Add(ResourceManager.Instance.MachineDatas.Find(val => val.MachineType == _machineType));
                }
            }
        }
    }

    public List<GlassRegistered> listGlassRegistered;
    public void AddController() => MainController.Instance.AddController(this);



}
