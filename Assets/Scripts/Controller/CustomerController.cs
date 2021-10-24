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
        [SerializeField] OrderController OrderController;
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
            OrderController = OrderController.Instance;

            // TODO
            maxSpawn = LevelController.LevelBase.minBuyer;

            getDepends();
        }

        public void OnGameStart()
        {
            //print("I'm already to spawn my child");
            SpawnerState = SpawnerState.CAN_CREATE;
        }

        private void Update()
        {
            if (gameState != GameState.START) return;

            if(SpawnerState == SpawnerState.CAN_CREATE && customerCounter <= maxSpawn)
            {
                SpawnerState = SpawnerState.VALIDATE;
                if (isAvaibleSeat())
                {
                    createCustomer(seatIndex[Random.Range(0, seatIndex.Count)]);
                }
            }
        }

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

        public void SetPlaceAvaibility(int _seatIndex, bool _isAvaible)
        {
        }

        public void OnCustomerDone(BuyerPrototype _cust)
        {
            TransformSeatDatas[_cust.seatIndex].isSeatAvaible = true;
        }

        public GameObject GetGameObject() => gameObject;

        public void OnGameIddle() { }

        public void OnGamePause()
        {
            throw new System.NotImplementedException();
        }

        public void OnGameClearance()
        {
            throw new System.NotImplementedException();
        }

        public void OnGameFinish()
        {
            throw new System.NotImplementedException();
        }

        public void OnGameInit()
        {
        }
    }
}
