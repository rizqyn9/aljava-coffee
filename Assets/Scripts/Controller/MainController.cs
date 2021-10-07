using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class MainController : Singleton<MainController>
    {
        [Header("Properties")]
        public int delay = 10;
        public GameObject tempCustomer;
        public int targetCustomer = 5;

        [Header("Debug")]
        [SerializeField] GameState _gameState;
        [SerializeField] LevelBase _levelBase;
        [SerializeField] bool isGameEnd = false;
        [SerializeField] List<IGameState> ListenGameState = new List<IGameState>();

        public static event Action<GameState> OnGameStateChanged;

        public LevelBase LevelBase { get => _levelBase; set => _levelBase = value; }

        #region GAME STATE
        public static GameState GameState {
            get => Instance._gameState;
            set
            {
                print($"<color=green> Game State Changed {value} </color>");
                OnGameStateChanged?.Invoke(value);
                Instance._gameState = value;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown("q"))
            {
                print("click");
                OnGameStateChanged?.Invoke(GameState);
            }
        }
        
        #endregion
        

        public void Init()
        {
            LevelController.LevelBase = LevelBase;

            GameState = GameState.INIT;
            print("<color=green>Init in Main Controller</color>");

            GameState = GameState.BEFORE_START;
            initAllControllers();

            StartCoroutine(IStartGame());
        }

        private static void initAllControllers()
        {
            //LevelController.Instance.Init();    //
            //EnvController.Instance.Init();      //  
            GameUIController.Instance.Init();   //
            OrderController.Instance.Init();    //
            CustomerController.Instance.Init(); //
            RulesController.Instance.Init();    //
        }

        IEnumerator IStartGame()
        {
            print("Game Started");
            GameState = GameState.BEFORE_START;

            //yield return new WaitForSeconds(1);
            //GameUIController.Instance.StartUI();

            ////Depreceated
            ////EnvController.Instance.StartMachine();

            //yield return new WaitForSeconds(3);

            //CustomerController.Instance.StartCustomer();
            yield break;
        }


        [SerializeField] EnvController EnvController;
        [SerializeField] LevelController LevelController;
        [SerializeField] GameUIController GameUIController;
        [SerializeField] OrderController OrderController;
        [SerializeField] CustomerController CustomerController;
        [SerializeField] RulesController RulesController;

        public void Start()
        {
            Application.targetFrameRate = 60; // Optional platform
            //GameState = GameState.FINISH;

            EnvController = EnvController.Instance;
            LevelController = LevelController.Instance;
            GameUIController = GameUIController.Instance;
            OrderController = OrderController.Instance;
            CustomerController = CustomerController.Instance;
            RulesController = RulesController.Instance;
        }

        IEnumerator gameFinished()
        {
            new WaitForSeconds(4);
            isGameEnd = true;
            yield break;
        }
    }
}
