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

        public void Init()
        {
            MainController.Instance.RegistGameState(this);

            LevelBase = LevelController.Instance.LevelBase;
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
    }
}
