using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public struct transformSeatData
    {
        public bool isSeatAvaible;
        public Transform transform;
    }

    public enum SpawnerState
    {
        IDDLE,
        REACTIVE,
        VALIDATE,
        CAN_CREATE,
        MAX_SEAT,
        MAX_ORDER
    }

    public class CustomerControler : Singleton<CustomerControler>, IController
    {
        [Header("Properties")]
        public transformSeatData[] TransformSeatDatas;
        public Vector2[] spawnPos;
        public GameObject tempCustomerPrefab;

        [Header("Debug")]
        [SerializeField] LevelBase LevelBase = null;
        [SerializeField] List<BuyerType> BuyerTypes = new List<BuyerType>();
        [SerializeField] List<MenuType> MenuTypes = new List<MenuType>();
        [SerializeField] ResourceCount ResourceCount;
        [SerializeField] int maxSpawn;
        [SerializeField] float delayCustomer;
        [SerializeField] SpawnerState SpawnerState = SpawnerState.IDDLE;
        [SerializeField] int customerCounter;


        [SerializeField]
        private GameState _gameState;
        public GameState GameState { get => _gameState; set => _gameState = value; }

        public void UpdateGameState(GameState _old, GameState _new) => GameState = _new;

        internal void Init()
        {
            MainController.Instance.AddController(this);

            print("Buyer Init");
            getDepends();
        }

        internal void StartCustomer()
        {
            print("I'm already to spawn my child");
            SpawnerState = SpawnerState.CAN_CREATE;
        }

        private void Update()
        {
            if (GameState != GameState.PLAY) return;

            if(SpawnerState == SpawnerState.CAN_CREATE)
            {
                SpawnerState = SpawnerState.VALIDATE;
                if (isAvaibleSeat())
                {
                    int set = seatIndex[Random.Range(0, seatIndex.Count)];
                    print(set);
                    createCustomer(set);
                }

            }

        }

        private void createCustomer(int _seatIndex)
        {
            TransformSeatDatas[_seatIndex].isSeatAvaible = false;

            GameObject custGO = Instantiate(tempCustomerPrefab, transform);
            CustomerHandler customer = custGO.GetComponent<CustomerHandler>();

            BuyerPrototype buyerPrototype = new BuyerPrototype();
            buyerPrototype.buyerType = BuyerTypes[Random.Range(0, ResourceCount.BuyerCount)];
            buyerPrototype.customerCode = $"Customer-{customerCounter++}";
            buyerPrototype.seatIndex = _seatIndex;
            buyerPrototype.menuListNames = generateMenu(Random.Range(1, 2));

            // reference buyer
            customer.initBuyer(buyerPrototype);

            StartCoroutine(IReactiveSpawner());
        }

        IEnumerator IReactiveSpawner()
        {
            SpawnerState = SpawnerState.REACTIVE;
            yield return new WaitForSeconds(4);
            SpawnerState = SpawnerState.CAN_CREATE;
            print("HI i'm spawner");
        }

        private void getDepends()
        {
            BuyerTypes = LevelController.Instance.BuyerTypes;
            MenuTypes = LevelController.Instance.MenuTypes;
            LevelBase = LevelController.Instance.LevelBase;
            ResourceCount = LevelController.Instance.ResourceCount;
            delayCustomer = LevelBase.delayPerCustomer;
        }

        [SerializeField] List<int> seatIndex;
        private bool isAvaibleSeat()
        {
            seatIndex = new List<int>();
            for (int i = 0; i < TransformSeatDatas.Length; i++)
            {
                if (TransformSeatDatas[i].isSeatAvaible) seatIndex.Add(i);
            }
            return seatIndex.Count > 0 ? true : false;
        }

        List<menuListName> generateMenu(int _total)
        {
            List<menuListName> res = new List<menuListName>();
            for (int i = 0; i < _total; i++)
            {
                res.Add(MenuTypes[Random.Range(0, ResourceCount.MenuCount)].menuListName);
            }
            return res;
        }
    }
}
