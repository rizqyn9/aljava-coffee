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
public class LevelController : Singleton<LevelController>, IGameState
{
    [Header("Debug")]
    [SerializeField] LevelBase _levelBase;
    public List<MenuType> MenuTypes = new List<MenuType>();
    public List<BuyerType> BuyerTypes = new List<BuyerType>();
    public List<MachineData> MachineDatas = new List<MachineData>();
    public List<MenuClassificationData> MenuClassificationDatas = new List<MenuClassificationData>();
    public ResourceCount ResourceCount;
    [SerializeField] GameState gameState;

    public static LevelBase LevelBase {
        get => Instance._levelBase;
        set { Instance._levelBase = value; Instance.setData(); }
    }

    private void setData()
    {
        MenuTypes = ResourceManager.ListMenu().FindAll(val => _levelBase.MenuTypeUnlock.Contains(val.menuListName));
        BuyerTypes = ResourceManager.ListBuyer().FindAll(val => _levelBase.BuyerTypeUnlock.Contains(val.enumBuyerType));
        MenuClassificationDatas = ResourceManager.ListMenuClass().FindAll(val => _levelBase.MenuClassifications.Contains(val.MenuClassification));

        ResourceCount = new ResourceCount()
        {
            BuyerCount = BuyerTypes.Count,
            MenuCount = MenuTypes.Count
        };

        GetMachineMustSpawn();
    }

    private void GetMachineMustSpawn()
    {
        List<MachineIgrendient> MachineTypes = new List<MachineIgrendient>();

        foreach (MenuType _menuType in MenuTypes)
        {
            foreach (MachineIgrendient _machineType in _menuType.Igrendients)
            {
                if (!MachineTypes.Contains(_machineType))
                {
                    MachineTypes.Add(_machineType);
                    MachineDatas.Add(ResourceManager.ListMachine().Find(val => val.machineType == _machineType));
                }
            }
        }
    }


    #region Game Controller
    private void OnEnable() => MainController.OnGameStateChanged += GameStateHandler;
    private void OnDisable() => MainController.OnGameStateChanged -= GameStateHandler;

    public void GameStateHandler(GameState _gameState)
    {
        gameState = _gameState;
        GameStateController.UpdateGameState(this, gameState);
    }

    public GameObject GetGameObject() => gameObject;

    public void OnGameIddle() { }

    public void OnGameBeforeStart() { }

    public void OnGameStart() { }

    public void OnGamePause() { }

    public void OnGameClearance() { }

    public void OnGameFinish() { }

    public void OnGameInit()
    {
        //throw new System.NotImplementedException();
    }

    #endregion
}
