using UnityEngine;

namespace Game
{
    public class RulesController : Singleton<RulesController>, IGameState
    {
        [Header("GameStat")]
        public GameMode gameMode;
        public float gameDuration = 0;

        public int buyerInstanceTotal = 0;
        public int buyerSuccessTotal = 0;
        public int buyerRunOutTotal = 0;

        public int menuInstanceTotal = 0;
        public int earnMoneyTotal = 0;

        [Header("Debug")]
        [SerializeField] LevelBase levelBase;
        [SerializeField] GameState gameState;
        public void OnGameStateChanged(GameState _old, GameState _new) => gameState = _new;

        private void OnEnable() => MainController.OnGameStateChanged += GameStateHandler;
        private void OnDisable() => MainController.OnGameStateChanged += GameStateHandler;
        public void Init()
        {
            levelBase = LevelController.LevelBase;
        }

        public void GameStateHandler(GameState _gameState) { }

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

        #region Rules Condition
        public void HandleGameTimeOut()
        {
            Debug.LogWarning("Game was Time Out");
            GameLose();
        }

        void GameWin()
        {

        }

        void GameLose()
        {

        }
        #endregion 

        public GameObject GetGameObject() => gameObject;
        public void OnGameIddle() { }
        public void OnGameBeforeStart() { }
        public void OnGameStart() { }
        public void OnGamePause() { }
        public void OnGameClearance() { }
        public void OnGameFinish() { }
        public void OnGameInit() { }
    }
}
