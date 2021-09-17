using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

[System.Serializable]
public struct buyerMenuPrototype
{
    public int numberID;
    public enumBuyerState buyerState;
    public BuyerType buyerType;
    public List<MenuType> menuTypes;
    public Buyer buyer;
}

public class BuyerManager : Singleton<BuyerManager>
{
    [Header("Properties")]
    public List<Transform> listSpawnTransform = new List<Transform>();
    public List<Transform> listStartTransform = new List<Transform>();
    public GameObject buyerPrefab;

    [Header("Demo")]
    public int buyerTotal;

    [Header("Debug")]
    public List<SpawnHandler> spawnHandlers = new List<SpawnHandler>();
    public int buyerTypeCount;
    public int menuTypeCount;
    public List<BuyerType> buyerTypes = new List<BuyerType>();
    public Dictionary<int, Buyer> buyerDict = new Dictionary<int, Buyer>();
    public List<buyerMenuPrototype> buyerList = new List<buyerMenuPrototype>();

    private void Start()
    {
        buyerTypeCount = ResourceManager.Instance.BuyerTypes.Count;
        menuTypeCount = ResourceManager.Instance.MenuTypes.Count;
        generateBuyer(buyerTotal);
        for(int i = 0; i < buyerList.Count; i++)
        {
            GameObject GO = Instantiate(buyerPrefab, listStartTransform[Random.Range(0, 2)]);
            Buyer buyer = GO.GetComponent<Buyer>();
            buyerDict.Add(i, buyer);
            buyer.buyerMenuPrototype = buyerList[i];
            buyer.spawnChar();
        }

        spawnLogic();
    }

    private void spawnLogic()
    {
        buyerDict[0].showToScene(listSpawnTransform[1].position);
    }

    public void generateBuyer(int _buyerTotal)
    {
        for(int i =0; i< _buyerTotal; i++)
        {
            buyerMenuPrototype prototype = new buyerMenuPrototype();
            prototype.numberID = i + 1;
            prototype.buyerType = findBuyerType();
            prototype.menuTypes = findMenuType(Random.Range(1,2));
            prototype.buyerState = enumBuyerState.ON_IDDLE;
            buyerList.Add(prototype);
        }
    }

    private List<MenuType> findMenuType(int _count)
    {
        List<MenuType> result = new List<MenuType>();
        for(int i = 0; i < _count; i++)
        {
            result.Add(ResourceManager.Instance.MenuTypes[Random.Range(0, menuTypeCount)]);
        }
        return result;
    }

    private BuyerType findBuyerType()
    {
        return ResourceManager.Instance.BuyerTypes[Random.Range(0, buyerTypeCount)];
    }
}
