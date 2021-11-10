using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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
        public GameObject PauseUI;
        public bool gameIsPaused = false;
        public Transform healthContainer;
        public GameObject healthPrefab;
        public Sprite healthRed, healthDark;

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

        private void Start()
        {
            PauseUI.SetActive(false);
        }

        public void OnGameInit()
        {
            setComponentUI();
        }

        public void OnGameBeforeStart()
        {
            instanceHealth(levelBase.healthTotal);

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

        #region Pause Handler
        public void Btn_Pause()
        {
            gameIsPaused = !gameIsPaused;
            PauseUI.SetActive(gameIsPaused);
            Time.timeScale = gameIsPaused ? 0f : 1f;
        }

        public void Btn_BackHome()
        {
            GameManager.Instance.backToHome();
        }
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
            if (CountDown <= 0) MainController.Instance.handleGameTimeOut();
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
            LeanTween.moveY(TopBar.GetComponent<RectTransform>(), 0, GlobalController.Instance.startingAnimLenght).setEaseOutBounce();
            LeanTween.moveX(PauseGO.GetComponent<RectTransform>() , -30f, GlobalController.Instance.startingAnimLenght).setEaseInOutBounce();
        }

        string getText() => $"{MainController.RulesController.buyerSuccessTotal} / {targetCounter} buyer";

        #region NoClickArea
        public void setNoClickArea(bool isActive) => noClickArea.SetActive(isActive);
        public void Btn_NoClickAre() => print("click");
        #endregion

        #region Win Controller
        public void initWinUI()
        {
            print("Wn");
            win_UI.gameObject.SetActive(true);
            win_UI.init();
        }

        #endregion

        #region Lose Controller
        public void initLoseUI()
        {
            print("Lose");
        }

        #endregion

        #region Health Controller

        [System.Serializable]
        public struct HealthStruct
        {
            public int index;
            public bool isActive;
            public Image image;
            public GameObject go;
        }

        [SerializeField] List<HealthStruct> healths = new List<HealthStruct>();
        [SerializeField] int healthActiveState = 0;
        public void instanceHealth(int _count)
        {
            for(int i = 0; i < _count; i++)
            {
                HealthStruct data = new HealthStruct
                {
                    go = Instantiate(healthPrefab, healthContainer),
                    index = i,
                    isActive = true
                };
                data.go.transform.localScale = Vector2.zero;
                data.go.GetComponent<RectTransform>().localPosition = new Vector2(i * 50f, 0); ;
                data.image = data.go.GetComponent<Image>();
                data.image.sprite = healthRed;
                healths.Add(data);
                LeanTween.scale(data.go.GetComponent<RectTransform>(), new Vector2(1f, 1f), .5f).setDelay(i * .5f);
            }
            healthActiveState = _count;
        }
        [ContextMenu("Test healt")]
        public void simDec() => changeHealth(true);
        public void changeHealth(bool _isDecreament = true)
        {
            HealthStruct target = healths[healthActiveState - 1];
            target.go.GetComponent<RectTransform>().LeanScale(new Vector2(.5f, .5f),.2f).setLoopPingPong(2);
            target.image.sprite = _isDecreament ? healthDark : healthRed;
            target.isActive = !_isDecreament;
            healthActiveState = _isDecreament ? healthActiveState - 1 : healthActiveState + 1;

            MainController.RulesController.OnHealthChanged(healthActiveState);
        }

        #endregion
    }
}
