using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace Game
{
    public class GameUIController : Singleton<GameUIController>, IGameState
    {
        public TMP_Text progressHandler;
        public TMP_Text timeUI;
        public float countDown = 120;
        public bool timerIsRunning = false;
        public bool timeOut = false;

        private void Update()
        {
            if (timerIsRunning)
            {
                if (countDown > 0)
                {
                    countDown -= Time.deltaTime;
                    timeUI.text = Mathf.Ceil(countDown).ToString();
                }
                else
                {
                    MainController.Instance.isGameTimeOut = true;

                }
            }
        }

        //private void Start()
        //{
        //    asOrderCount();
        //}

        [SerializeField] int _counter = 0;
        [SerializeField] GameState _gameState;
        public GameState GameState
        {
            get => _gameState;
            set
            {
                _gameState = value;
            }
        }
        public void OnGameStateChanged() => GameState = MainController.Instance.GameState;

        public void asOrderCount()
        {
            Debug.Log("asd");
            progressHandler.text = $"{_counter} / {MainController.Instance.targetCustomer} Orders";
            _counter = _counter += 1;
        }

        internal void Init()
        {
            MainController.Instance.RegistGameState(this);
        }

        internal void StartUI()
        {
        }
    }
}
