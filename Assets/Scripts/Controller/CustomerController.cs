using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(OrderController))]
    public class CustomerController : Singleton<CustomerController>, IGameState
    {
        [Header("Properties")]
        public transformSeatData[] TransformSeatDatas;
        public Vector2[] spawnPos;
        public GameObject tempCustomerPrefab;

        [Header("Debug")]
        [SerializeField] OrderController OrderController;
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
        public GameState GameState
        {
            get => _gameState;
            set
            {
                _gameState = value;
            }
        }
        public void OnGameStateChanged() => GameState = MainController.Instance.GameState;


        internal void Init()
        {
            MainController.Instance.RegistGameState(this);
            OrderController = OrderController.Instance;

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
                    createCustomer(seatIndex[Random.Range(0, seatIndex.Count)]);
                }
            }
        }

        Vector2[] spawnPosTemp = { new Vector2(-8.11999989f, 0), new Vector2(12.4099998f, 0) };
        private void createCustomer(int _seatIndex)
        {
            TransformSeatDatas[_seatIndex].isSeatAvaible = false;

            GameObject custGO = Instantiate(tempCustomerPrefab, spawnPosTemp[Random.Range(0, 2)], Quaternion.identity, transform);
            CustomerHandler customer = custGO.GetComponent<CustomerHandler>();

            BuyerPrototype buyerPrototype = new BuyerPrototype()
            {
                buyerType       = BuyerTypes[Random.Range(0, ResourceCount.BuyerCount)],
                customerCode    = $"Customer-{customerCounter++}",
                seatIndex       = _seatIndex,
                menuListNames   = getMenuTypes(Random.Range(1, 2))
            };

            // reference buyer
            customer.initBuyer(buyerPrototype);

            StartCoroutine(IReactiveSpawner());
        }

        IEnumerator IReactiveSpawner()
        {
            SpawnerState = SpawnerState.REACTIVE;
            yield return new WaitForSeconds(4);
            SpawnerState = SpawnerState.CAN_CREATE;
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
        
        List<MenuType> getMenuTypes(int _total)
        {
            List<MenuType> res = new List<MenuType>();
            for (int i = 0; i < _total; i++)
            {
                res.Add(MenuTypes[Random.Range(0, ResourceCount.MenuCount)]);
            }
            return res;
        }
    }
}
