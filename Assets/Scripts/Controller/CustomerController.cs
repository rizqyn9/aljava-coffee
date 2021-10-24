using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(OrderController))]
    public class CustomerController : Singleton<CustomerController>, IGameState
    {
        [Header("Properties")]
        public Vector2[] spawnPos;
        public GameObject tempCustomerPrefab;
        [SerializeField] transformSeatData[] TransformSeatDatas;

        [Header("Debug")]
        [SerializeField] OrderController orderController;
        [SerializeField] LevelBase LevelBase = null;
        [SerializeField] List<BuyerType> BuyerTypes = new List<BuyerType>();
        [SerializeField] List<MenuType> MenuTypes = new List<MenuType>();
        [SerializeField] ResourceCount ResourceCount;
        [SerializeField] int maxSpawn;
        [SerializeField] float delayCustomer;
        [SerializeField] SpawnerState SpawnerState = SpawnerState.IDDLE;
        [SerializeField] int customerCounter;
        [SerializeField] GameState gameState;

        private void OnEnable() => MainController.OnGameStateChanged += GameStateHandler;

        private void OnDisable() => MainController.OnGameStateChanged += GameStateHandler;

        public void GameStateHandler(GameState _gameState)
        {
            gameState = _gameState;
            GameStateController.UpdateGameState(this, gameState);
        }

        public void OnGameBeforeStart()
        {
            orderController = OrderController.Instance;

            // TODO
            maxSpawn = LevelController.LevelBase.minBuyer;

            getDepends();
        }

        #region Reactive Spawn Logic

        public void OnGameStart()
        {
            SpawnerState = SpawnerState.CAN_CREATE;
        }

        private void Update()
        {
            if (gameState != GameState.START) return;

            if(SpawnerState == SpawnerState.CAN_CREATE && customerCounter <= maxSpawn)
            {
                if (isAvaibleSeat())
                {
                    customerPresence(instance: 1);
                    createCustomer(seatIndex[Random.Range(0, seatIndex.Count)]);
                }
            }
        }

        IEnumerator IReactiveSpawner()
        {
            SpawnerState = SpawnerState.REACTIVE;
            yield return new WaitForSeconds(MainController.Instance.LevelBase.delayPerCustomer);
            SpawnerState = SpawnerState.CAN_CREATE;
        }

        #endregion

        #region Buyer Factory
        [SerializeField] Vector2[] spawnPosTemp;
        private void createCustomer(int _seatIndex)
        {
            BuyerPrototype buyerPrototype = new BuyerPrototype()
            {
                buyerType       = BuyerTypes[Random.Range(0, ResourceCount.BuyerCount)],
                customerCode    = $"Customer-{customerCounter++}",
                seatIndex       = _seatIndex,
                menuListNames   = getMenuTypes(Random.Range(1, 2)),
                spawnPos        = spawnPosTemp[Random.Range(0, 2)],
                seatPos         = TransformSeatDatas[_seatIndex].transform.position
            };

            TransformSeatDatas[_seatIndex].isSeatAvaible = false;

            GameObject custGO = Instantiate(tempCustomerPrefab, buyerPrototype.spawnPos, Quaternion.identity, transform);
            CustomerHandler customer = custGO.GetComponent<CustomerHandler>();

            // reference buyer
            customer.initBuyer(buyerPrototype);

            StartCoroutine(IReactiveSpawner());
        }

        #endregion

        [SerializeField] int buyerInstanceTotal = 0;
        [SerializeField] int buyerSuccessTotal = 0;
        [SerializeField] int buyerRunOutTotal = 0;
        public void customerPresence(
            int instance = 0,
            int success = 0,
            int runOut = 0
            )
        {
            buyerInstanceTotal += instance;
            buyerSuccessTotal += success;
            buyerRunOutTotal += runOut;

            RulesController.Instance.customerPresence(instance, success, runOut);
        }

        private void getDepends()
        {
            BuyerTypes = LevelController.Instance.BuyerTypes;
            MenuTypes = LevelController.Instance.MenuTypes;
            LevelBase = LevelController.LevelBase;
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

        public void OnLeave(BuyerPrototype _cust)
        {
            TransformSeatDatas[_cust.seatIndex].isSeatAvaible = true;
        }

        #region GAME STATE
        public GameObject GetGameObject() => gameObject;

        public void OnGameIddle() { }

        public void OnGamePause() { }

        public void OnGameClearance() { }

        public void OnGameFinish() { }

        public void OnGameInit() { }

        #endregion
    }
}
