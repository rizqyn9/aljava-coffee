using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace Game
{
    public class GameUIController : Singleton<GameUIController>, IGameState, IEnv
    {
        public GameObject topBar;
        public TMP_Text progressHandler;
        public TMP_Text timeUI;
        public float countDown = 120;
        public bool timerIsRunning = false;
        public bool timeOut = false;

        [Header("Debug")]
        [SerializeField] Vector2 basePos;

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

        [SerializeField] bool go;
        IEnumerator ITimer()
        {
            while (go)
            {
                yield return new WaitForSeconds(1);
                sec += 1;
            }
            yield break;
        }

        [SerializeField] int sec;
        private void Start()
        {
            EnvController.RegistEnv(this);
            //basePos = topBar.transform.position;
            //topBar.transform.position = new Vector2(0 ,-2f);

            StartCoroutine(ITimer());
        }

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
