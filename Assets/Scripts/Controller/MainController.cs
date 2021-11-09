using System;
using System.Collections;
using UnityEngine;

namespace Game
{
    public class MainController : Singleton<MainController>
    {
        [Header("Properties")]
        public int delay = 10;
        public GameObject tempCustomer;
        public int targetCustomer = 5;

        public static RulesController RulesController;

        [Header("Debug")]
        [SerializeField] GameState _gameState;
        [SerializeField] bool isGameEnd = false;
        public LevelBase levelBase;
        public InLevelUserData inLevelUserData;

        public static event Action<GameState> OnGameStateChanged;

        #region Prevent sprite
        private bool _onUI = false;
        public bool onUI
        {
            get => _onUI;
            set => _onUI = value;
        }
        #endregion

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
        #endregion


        public void Init(LevelBase _levelBase, InLevelUserData _inLevelUserData)
        {
            // Level Generator
            levelBase = _levelBase;
            inLevelUserData = _inLevelUserData;

            LevelController.LevelBase = _levelBase;
            RulesController = FindObjectOfType<RulesController>();

            print("<color=green>Init in Main Controller</color>");

            StartCoroutine(IStartGame());
        }

        IEnumerator IStartGame()
        {
            GameState = GameState.INIT;

            GameState = GameState.BEFORE_START;
            if (Dev.Instance.isDevMode)
            {
                yield return new WaitForSeconds(GlobalController.Instance.countingBeforeStart);
            }

            yield return new WaitForSeconds(GlobalController.Instance.startingAnimLenght);
            GameState = GameState.START;

            yield break;
        }

        public void Start()
        {
            Application.targetFrameRate = 30; // Optional platform
        }

        public void handleGameTimeOut()
        {
            RulesController.isTimeOut = true;
            GameState = GameState.CLEARANCE;
        }

        public bool canInstanceWinLoseController = false;
        public void handleGameClearance()
        {
            canInstanceWinLoseController = true;
            GameState = GameState.FINISH;
        }
    }
}
