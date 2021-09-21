using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

namespace Game
{
    public class GameUIController : Singleton<GameUIController>
    {
        public TMP_Text progressHandler;
        public TMP_Text timeUI;
        public float countDown = 120;
        public bool timerIsRunning = false;
        public bool timeOut = false;
        public GameObject winPrefab;
        public GameObject losePrefab;


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

        public void finalize(bool _isWin)
        {
            if (_isWin) instanceWin();
            else instanceLose();
        }

        private void instanceLose()
        {
            losePrefab.SetActive(true);
        }

        private void instanceWin()
        {
            winPrefab.SetActive(true);
        }

        private void Start()
        {
            asOrderCount();
        }

        [SerializeField] int _counter = 0;
        public void asOrderCount()
        {
            Debug.Log("asd");
            progressHandler.text = $"{_counter} / {MainController.Instance.targetCustomer} Orders";
            _counter = _counter += 1;
        }

    }
}
