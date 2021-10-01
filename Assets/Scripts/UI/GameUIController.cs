using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace Game
{
    public class GameUIController : Singleton<GameUIController>, IGameState, IEnv
    {
        [Header("Properties")]
        [SerializeField] GameObject PauseGO;
        [SerializeField] GameObject TopBar;
        [SerializeField] float offsetTopBar = 40;
        public TMP_Text progressHandler;
        public TMP_Text timeUI;
        public bool timerIsRunning = false;

        [Header("Debug")]
        [SerializeField] Vector2 basePos;
        [SerializeField] LevelBase LevelBase;


        #region GAME STATE
        [SerializeField] GameState _gameState;
        public GameState GameState
        {
            get => _gameState;
            set {
                _gameState = value;
            }
        }
        public void OnGameStateChanged() => GameState = MainController.Instance.GameState;
        #endregion

        #region COUNTDOWN CONTROLLER
        [SerializeField] int _countDown = 0;
        int CountDown
        {
            get => _countDown;
            set
            {
                _countDown = value;
                timeUI.text = _countDown.ToString();
            }
        }

        #endregion
        
        IEnumerator ITimer()
        {
            while (CountDown > 0)
            {
                timerIsRunning = true;
                yield return new WaitForSeconds(1);
                CountDown -= 1;
            }
            timerIsRunning = false;
            if (CountDown <= 0) RulesController.Instance.HandleGameTimeOut();
            yield break;
        }

        void StartGame()
        {
            print("Start Timer");
            StartCoroutine(ITimer());

        }

        #region Top Bar Controller
        [SerializeField] int targetCounter;
        [SerializeField] int _counter;
        public int Counter {
            get => _counter;
            set
            {
                if (_counter == value) return;
                _counter = value;
                updateUI();
            }
        }

        private void updateUI()
        {
            progressHandler.text = getText();
        }

        #endregion

        internal void Init()
        {
            MainController.Instance.RegistGameState(this);
            EnvController.RegistEnv(this);
            setComponentUI();
        }

        private void setComponentUI()
        {
            LevelBase = LevelController.LevelBase;
            CountDown = LevelBase.gameDuration;

            Counter = 0;
            targetCounter = LevelBase.minBuyer;
            updateUI();

            TopBar.transform.LeanMoveLocalY(0, 1).setEaseInOutBounce();
            PauseGO.transform.LeanMoveLocalX(360, 1).setEaseInOutBounce();
        }

        internal void StartUI()
        {
            StartGame();
        }

        public void EnvInstance()
        {
            //topBar.transform.LeanMoveLocalY(0, 1f).setEaseInElastic();
        }

        public void Command()
        {
            throw new NotImplementedException();
        }

        string getText()
        {
            return $"{Counter} / {targetCounter} buyer";
        }
    }
}
