using System;
using UnityEngine;

namespace Game
{
    public class RulesController : MonoBehaviour, IGameState
    {
        [Header("GameStat")]
        public GameMode gameMode;
        public float gameDuration = 0;

        public int buyerInstanceTotal = 0;
        public int buyerSuccessTotal = 0;
        public int buyerRunOutTotal = 0;

        public int menuInstanceTotal = 0;
        public int earnMoneyTotal = 0;
        public int point;

        public int healthActive;
        public bool isTimeOut;
        public bool isWin = false;

        public int starTotal;

        [Header("Debug")]
        [SerializeField] LevelBase levelBase;
        [SerializeField] GameState gameState;

        private void OnEnable() => MainController.OnGameStateChanged += GameStateHandler;
        private void OnDisable() => MainController.OnGameStateChanged += GameStateHandler;

        public void GameStateHandler(GameState _gameState)
        {
            gameState = _gameState;
            GameStateController.UpdateGameState(this, gameState);
        }

        public void customerPresence(
            int instance = 0,
            int success = 0,
            int runOut = 0
            )
        {
            buyerInstanceTotal += instance;
            buyerSuccessTotal += success;
            buyerRunOutTotal += runOut;

            GameUIController.Instance.OnUpdatePresence();
        }

        public void OnHealthChanged(int _healthActiveState)
        {
            healthActive = _healthActiveState;
            if (_healthActiveState == 0)
            {
                MainController.Instance.handleGameClearance();
            };
        }

        #region Rules Condition

        private void calcPoint()
        {
            calcByBuyer();
        }

        private void calcByBuyer()
        {
            float val = buyerSuccessTotal / levelBase.minBuyer * 100;
            starTotal =
                val >= 100 ? 3 :
                val >= 75 ? 2 :
                val >= 45 ? 1 :
                0;
        }

        void GameWin()
        {
            GameUIController.Instance.initWinUI();
            GameManager.Instance.saveData.updateLevel(
                getLevelModel(true)
            );
        }

        void GameLose()
        {
            GameUIController.Instance.initLoseUI();
            GameManager.Instance.saveData.updateLevel(
                getLevelModel(false)
            );
        }

        private LevelModel getLevelModel(bool _isWin)
        {
            return new LevelModel
            {
                isOpen = true,
                level = levelBase.level,
                stars = starTotal,
                playerInstance = buyerInstanceTotal,
                point = point,
                isWin = _isWin
            };
        }

        #endregion 

        public GameObject GetGameObject() => gameObject;
        public void OnGameIddle() { }
        public void OnGameBeforeStart()
        {
            levelBase = LevelController.LevelBase;
        }
        public void OnGameStart() { }
        public void OnGamePause() { }
        public void OnGameClearance() { }

        public void OnGameFinish()
        {
            calcPoint();
            if (isWin || isTimeOut)
            {
                GameWin();
            } else
            {
                GameLose();
            }
        }

        public void OnGameInit() { }
    }
}
