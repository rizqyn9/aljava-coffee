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
        public TMP_Text progressHandler;
        public TMP_Text timeUI;
        public float countDown = 120;
        public bool timerIsRunning = false;

        [Header("Debug")]
        [SerializeField] Vector2 basePos;
        [SerializeField] LevelBase LevelBase;

        [SerializeField] int _counter = 0;
        [SerializeField] GameState _gameState;
        public GameState GameState
        {
            get => _gameState;
            set {
                _gameState = value;
            }
        }
        public void OnGameStateChanged() => GameState = MainController.Instance.GameState;

        IEnumerator ITimer()
        {
            while (countDown > 0)
            {
                timerIsRunning = true;
                timeUI.text = countDown.ToString();
                yield return new WaitForSeconds(1);
                countDown -= 1;
            }
            timerIsRunning = false;
            if(countDown <= 0) 
            yield break;
        }

        void StartGame()
        {
            print("Start Timer");
            StartCoroutine(ITimer());

        }


        public void asOrderCount()
        {
            Debug.Log("asd");
            progressHandler.text = $"{_counter} / {MainController.Instance.targetCustomer} Orders";
            _counter = _counter += 1;
        }

        internal void Init()
        {
            print("Init UI");
            MainController.Instance.RegistGameState(this);
            EnvController.RegistEnv(this);

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
    }
}
