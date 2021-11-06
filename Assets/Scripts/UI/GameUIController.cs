using System.Collections;
using UnityEngine;
using TMPro;

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
        [SerializeField] GameObject noClickArea;
        [SerializeField] Win_UI win_UI; 
        public MachineOverlay machineOverlay;
        public Transform radiusUI;
        public Transform capacityUI;

        [Header("Debug")]
        [SerializeField] bool timerIsRunning = false;
        [SerializeField] Vector2 basePos;
        [SerializeField] LevelBase levelBase;

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
            machineOverlay.defaultPosition();
            machineOverlay.gameObject.SetActive(true);

            spawnTopUI();
        }

        public void OnGameStart()
        {
            StartCoroutine(ICountDown());
        }


        public GameObject GetGameObject() => gameObject;
        public void OnGameIddle() { }
        public void OnGamePause() { }
        public void OnGameClearance() { }
        public void OnGameFinish() { }
        #endregion

        #region Listen on Presence

        public void OnUpdatePresence()
        {
            updateUI();
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
        
        IEnumerator ICountDown()
        {
            while (CountDown > 0)
            {
                timerIsRunning = true;
                yield return new WaitForSeconds(1);
                CountDown -= 1;
            }
            timerIsRunning = false;
            if (CountDown <= 0) MainController.RulesController.HandleGameTimeOut();
            yield break;
        }

        #endregion

        #region Top Bar Controller
        [SerializeField] int targetCounter;

        private void updateUI()
        {
            progressHandler.text = getText();
        }

        #endregion

        private void setComponentUI()
        {
            levelBase = LevelController.LevelBase;
            CountDown = levelBase.gameDuration;

            targetCounter = levelBase.minBuyer;
            updateUI();
        }

        private void spawnTopUI()
        {
            TopBar.transform.LeanMoveLocalY(0, GlobalController.Instance.startingAnimLenght).setEaseInOutBounce();
            PauseGO.transform.LeanMoveLocalX(360, GlobalController.Instance.startingAnimLenght).setEaseInOutBounce();
        }

        string getText() => $"{MainController.RulesController.buyerSuccessTotal} / {targetCounter} buyer";

        #region NoClickArea
        public void setNoClickArea(bool isActive) => noClickArea.SetActive(isActive);
        public void Btn_NoClickAre() => print("click");
        #endregion

        #region Win Controller
        public void initWinUI()
        {
            win_UI.gameObject.SetActive(true);
            win_UI.init();
        }
        
        #endregion

        [ContextMenu("Simulate Win")]
        public void simWin()
        {
            Debug.LogWarning("WIN SIMULATE");
            initWinUI();
        }

        [ContextMenu("Simulate Lose")]
        public void simLose()
        {

        }
    }
}
