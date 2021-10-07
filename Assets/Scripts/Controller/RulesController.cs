using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class RulesController : Singleton<RulesController>, IGameState
    {
        [Header("Debug")]
        [SerializeField] LevelBase LevelBase;
        [SerializeField] GameState gameState;
        public void OnGameStateChanged(GameState _old, GameState _new) => gameState = _new;

        private void OnEnable()
        {
            MainController.OnGameStateChanged += GameStateHandler;
        }

        private void OnDisable()
        {
            MainController.OnGameStateChanged += GameStateHandler;
        }

        public void GameStateHandler(GameState _gameState)
        {

        }

        public void Init()
        {

            LevelBase = LevelController.LevelBase;
        }

        public void HandleGameTimeOut()
        {
            print("Game was time out!!");
            GameLose();
        }

        void GameWin()
        {

        }

        void GameLose()
        {

        }

        [SerializeField] int buyerCounter = 0;
        public static void OnCustomerServed(BuyerPrototype _buyerPrototype)
        {
            Instance.buyerCounter += 1;
            Instance.updateUI();
        }

        private void updateUI()
        {
            GameUIController.Instance.Counter = buyerCounter;
        }

        [SerializeField] int menuCounter = 0;
        public static void OnMenuServed(LevelBase _levelBase)
        {
            Instance.menuCounter += 1;
        }

        public GameObject GetGameObject() => gameObject;

        public void OnGameIddle()
        {
            throw new NotImplementedException();
        }

        public void OnGameBeforeStart()
        {
            throw new NotImplementedException();
        }

        public void OnGameStart()
        {
            throw new NotImplementedException();
        }

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

        public void OnGameInit()
        {
            throw new NotImplementedException();
        }
    }
}
