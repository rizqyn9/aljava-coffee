using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public interface IController
    {
        public void GameStateChanged(GameState _old, GameState _new);
    }

    public class MainController : Singleton<MainController>
    {
        [Header("Properties")]
        public transformSeatData[] seatDataTransform;
        public Transform[] startSpawnTransform;
        public int delay = 10;
        public GameObject tempCustomer;
        public int targetCustomer = 5;

        [Header("Debug")]
        [SerializeField] GameState _gameState;
        [SerializeField] LevelBase _levelBase;
        public List<BuyerPrototype> deliveryQueueMenu = new List<BuyerPrototype>();
        public int maxSlotOrder;
        public bool isDeliveryFull = false;
        public bool isGameStarted = false;
        public bool isGameFinished = false;
        public bool isGameTimeOut = false;
        public bool canCreateCustomer = false;
        [SerializeField] int customerCounter = 1;
        [SerializeField] bool isGameEnd = false;
        [SerializeField] List<IController> Controllers = new List<IController>();

        public LevelBase LevelBase { get => _levelBase; set => _levelBase = value; }

        #region GAME STATE
        public GameState GameState {
            get => _gameState;
            set
            {
                GameStateChanged(_gameState, value);
                _gameState = value;
            }
        }

        private void GameStateChanged(GameState _old, GameState _new)
        {
            if (_old == _new) return;
            print("Game state Changed");
            foreach(IController _controller in Controllers)
            {
                _controller.GameStateChanged(_old, _new);
            }
        }

        #endregion

        public void Init()
        {
            GameState = GameState.IDDLE;
            print("<color=green>Init in Main Controller</color>");

            LevelController.Instance.LevelBase = LevelBase;

            LevelController.Instance.Init();
            EnvController.Instance.Init();
            GameUIController.Instance.Init();
            CustomerController.Instance.Init();

            StartCoroutine(IStartGame());
        }

        [SerializeField] int controllerCount;
        public void AddController(IController _controller)
        {
            Controllers.Add(_controller);
            controllerCount = Controllers.Count;
        }

        IEnumerator IStartGame()
        {
            print("Game Started");
            GameState = GameState.PLAY;

            GameUIController.Instance.StartUI();
            EnvController.Instance.StartMachine();

            yield return new WaitForSeconds(1);

            CustomerController.Instance.StartCustomer();
            yield break;
        }

        public void Start()
        {
            Application.targetFrameRate = 60; // Optional platform

            GameState = GameState.IDDLE;

            maxSlotOrder = seatDataTransform.Length;

            //initMachine();
            //GameUIController.Instance.timerIsRunning = true;
            //StartCoroutine(onStart());
        }

        //private void initMachine()
        //{
        //    foreach(Machine _ in Machines)
        //    {
        //        _.isGameStarted = true;
        //        _.startMachine();
        //    }
        //}

        #region DEPRECEATED
        //IEnumerator onStart()
        //{
        //    yield return new WaitForSeconds(4);

        //    isGameStarted = true;
        //    canCreateCustomer = true;
        //}
        #endregion

        /// <summary>
        /// finding Machine target, with out Machine
        /// </summary>
        /// <param name="_machine"></param>
        /// <param name="_">out, bring out Machine class</param>
        /// <returns></returns>
        //public bool isMachineAvaible(MachineType _machine, out Machine _)
        //{
        //    _ = Machines.Find(val => val.machineType == _machine && val.MachineState == MachineState.ON_IDDLE);
        //    return _;
        //}

        private void Update()
        {
            if (isGameTimeOut)
            {
                StartCoroutine(gameFinished());
                return;
            }
            isGameFinished = deliveryQueueMenu.Count == targetCustomer;
            if (isGameFinished && !isGameEnd)
            {
                //if(freeSeatDataIndex.Count == seatDataTransform.Length)
                //{
                //    StartCoroutine(gameFinished());
                //    return;
                //}
            }

            if (deliveryQueueMenu.Count >= maxSlotOrder)
                isDeliveryFull = true;
            else
                isDeliveryFull = false;
        }

        IEnumerator gameFinished()
        {
            new WaitForSeconds(4);
            isGameEnd = true;
            yield break;
        }

        public bool findMenu(MenuType _menu, out CustomerHandler _customerHandler)
        {
            bool res = false;
            _customerHandler = null;
            for(int i= 0; i< deliveryQueueMenu.Count; i++)
            {
                res = deliveryQueueMenu[i].menuListNames.Contains(_menu);
                if (res)
                {
                    Debug.Log("Found Menu in Queue");
                    _customerHandler = deliveryQueueMenu[i].customerHandler;
                    break;
                }
            }
            return res;
        }

        public bool isExistQueue(MenuType _menuName, out BuyerPrototype _buyerPrototype)
        {
            _buyerPrototype = new BuyerPrototype();
            try
            {
                for(int i =0; i<deliveryQueueMenu.Count; i++)
                {
                    if (deliveryQueueMenu[i].menuListNames.Contains(_menuName))
                    {
                        _buyerPrototype = deliveryQueueMenu[i];
                        _buyerPrototype.menuListNames.Remove(_menuName);
                        if(_buyerPrototype.menuListNames.Count < 1)
                        {
                            Debug.Log("Clear");
                            _buyerPrototype.customerHandler.onMenusDone();
                        }
                        clearQueue(deliveryQueueMenu[i]);
                        return true;
                    }
                }
                throw new System.Exception();
            } catch
            {
                return false;
            }
        }

        private void clearQueue(BuyerPrototype _buyerPrototype)
        {
            
        }
    }
}
