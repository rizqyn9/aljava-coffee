using System.Collections;
using UnityEngine;
using TMPro;
using System;

namespace Game
{
    public class GameUIController : Singleton<GameUIController>, IGameState
    {
        [Header("Properties")]
        [SerializeField] GameObject PauseGO;
        [SerializeField] GameObject TopBar;
        [SerializeField] float offsetTopBar = 40;
        [SerializeField] TMP_Text progressHandler;
        [SerializeField] TMP_Text timeUI;
        [SerializeField] bool timerIsRunning = false;
        [SerializeField] GameObject MachineUIOverlay;

        [Header("Debug")]
        [SerializeField] Vector2 basePos;
        [SerializeField] LevelBase LevelBase;

        #region GAME STATE
        [SerializeField] GameState gameState;

        private void OnEnable() => MainController.OnGameStateChanged += GameStateHandler;
        private void OnDisable() => MainController.OnGameStateChanged += GameStateHandler;

        public void GameStateHandler(GameState _gameState)
        {
            gameState = _gameState;
            GameStateController.UpdateGameState(this, gameState);
        }

        public void OnGameInit()
        {
            setComponentUI();
        }

        public void OnGameBeforeStart()
        {
            spawnTopUI();
        }

        public void OnGameStart()
        {
            StartUI();
        }



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

        public GameState GameState { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private void updateUI()
        {
            progressHandler.text = getText();
        }

        #endregion
        internal void Init()
        {
        }

        private void setComponentUI()
        {
            LevelBase = LevelController.LevelBase;
            CountDown = LevelBase.gameDuration;

            Counter = 0;
            targetCounter = LevelBase.minBuyer;
            updateUI();
            //spawnTopUI();
        }

        private void spawnTopUI()
        {
            TopBar.transform.LeanMoveLocalY(0, 1).setEaseInOutBounce();
            PauseGO.transform.LeanMoveLocalX(360, 1).setEaseInOutBounce();
        }

        internal void StartUI()
        {
            StartGame();
        }

        string getText()
        {
            return $"{Counter} / {targetCounter} buyer";
        }

        public GameObject GetGameObject() => gameObject;

        public void OnGameIddle() { }

        public void OnGamePause()
        {
            throw new NotImplementedException();
        }

        public void OnGameClearance()
        {
            throw new NotImplementedException();
        }

        public void OnGameFinish()
        {
            throw new NotImplementedException();
        }


        [SerializeField] bool isOverlayActive;
        public bool reqUseMachineOverlay(GameObject _go)
        {
            if (isOverlayActive) return false;

            MachineUIOverlay.SetActive(true);
            MachineUIOverlay.LeanMoveLocalY(0, 2f);

            isOverlayActive = true;
            return true;
        }

        public void Btn_CloseOverlayMachine()
        {
            if (!isOverlayActive) return;
            MachineUIOverlay.LeanMoveLocalY(-430, 2f);

            isOverlayActive = false;
        }
    }
}
