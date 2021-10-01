using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface IGameState
    {
        public void OnGameStateChanged();
        public GameState GameState { get; set; }
    }

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
        [SerializeField] List<IGameState> ListenGameState = new List<IGameState>();

        public LevelBase LevelBase { get => _levelBase; set => _levelBase = value; }

        #region GAME STATE
        public GameState GameState {
            get => _gameState;
            set
            {
                GameStateChanged(_gameState, value);
                _gameState = value;
            }
        }

        private void GameStateChanged(GameState _old, GameState _new)
        {
            if (_old == _new) return;
            print("Game state Changed");
            foreach(IGameState _state in ListenGameState)
            {
                _state.GameState = _new;
            }
        }

        #endregion

        public void Init()
        {
            ListenGameState.ForEach((val) =>
            {
                val.GameState = GameState.PAUSE;
            });
            GameState = GameState.IDDLE;
            print("<color=green>Init in Main Controller</color>");

            LevelController.LevelBase = LevelBase;

            initAlController();

            StartCoroutine(IStartGame());
        }

        private static void initAlController()
        {
            LevelController.Instance.Init();
            EnvController.Instance.Init();
            GameUIController.Instance.Init();
            OrderController.Instance.Init();
            CustomerController.Instance.Init();
            RulesController.Instance.Init();
        }

        [SerializeField] int countIGameState;
        public void RegistGameState(IGameState _)
        {
            ListenGameState.Add(_);
            countIGameState = ListenGameState.Count;
        }

        IEnumerator IStartGame()
        {
            print("Game Started");
            GameState = GameState.PLAY;

            yield return new WaitForSeconds(1);
            EnvController.Instance.instanceIEnv();
            GameUIController.Instance.StartUI();
            EnvController.Instance.StartMachine();

            yield return new WaitForSeconds(3);

            CustomerController.Instance.StartCustomer();
            yield break;
        }

        public void Start()
        {
            Application.targetFrameRate = 60; // Optional platform

            GameState = GameState.IDDLE;
        }

        IEnumerator gameFinished()
        {
            new WaitForSeconds(4);
            isGameEnd = true;
            yield break;
        }
    }
}
