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

        public static event Action<GameState> OnGameStateChanged;

        public LevelBase LevelBase { get => _levelBase; set => _levelBase = value; }

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

        private void Update()
        {
        }
        
        #endregion


        public void Init()
        {
            LevelController.LevelBase = LevelBase;

            print("<color=green>Init in Main Controller</color>");

            StartCoroutine(IStartGame());
        }

        IEnumerator IStartGame()
        {
            GameState = GameState.INIT;

            //yield return new WaitForSeconds(1);

            GameState = GameState.BEFORE_START;

            yield return new WaitForSeconds(GlobalController.Instance.startingAnimLenght);
            GameState = GameState.START;

            yield break;
        }

        public void Start()
        {
            Application.targetFrameRate = 30; // Optional platform
        }

        IEnumerator gameFinished()
        {
            new WaitForSeconds(4);
            isGameEnd = true;
            yield break;
        }
    }
}
