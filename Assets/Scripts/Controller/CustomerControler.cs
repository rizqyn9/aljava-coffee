using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public struct transformSeatData
    {
        public bool isSeatAvaible;
        public Transform transform;
    }

    public enum SpawnerState
    {
        IDDLE,
        REACTIVE,
        CAN_CREATE,
        MAX_SEAT,
        MAX_ORDER
    }

    public class CustomerControler : Singleton<CustomerControler>, IController
    {
        [Header("Properties")]
        public transformSeatData[] TransformSeatDatas;
        public Vector2[] spawnPos;

        [Header("Debug")]
        [SerializeField] LevelBase LevelBase = null;
        [SerializeField] List<BuyerType> BuyerTypes = new List<BuyerType>();
        [SerializeField] List<MenuType> MenuTypes = new List<MenuType>();
        [SerializeField] int maxSpawn;
        [SerializeField] GameState GameState;
        [SerializeField] float delayCustomer;
        [SerializeField] SpawnerState SpawnerState = SpawnerState.IDDLE;

        internal void Init()
        {
            print("Buyer Init");
            getDepends();
        }

        internal void StartCustomer()
        {
            print("I'm already to spawn my child");
            SpawnerState = SpawnerState.CAN_CREATE;
        }

        private void Update()
        {
            if (GameState != GameState.PLAY) return;

            if(SpawnerState == SpawnerState.CAN_CREATE)
            {
                print("Yuhu im a customer");
                StartCoroutine(IReactiveSpawner());

            }

        }

        IEnumerator IReactiveSpawner()
        {
            SpawnerState = SpawnerState.REACTIVE;
            yield return new WaitForSeconds(delayCustomer);
            SpawnerState = SpawnerState.CAN_CREATE;
            print("HI i'm spawner");
        }

        private void getDepends()
        {
            BuyerTypes = LevelController.Instance.BuyerTypes;
            MenuTypes = LevelController.Instance.MenuTypes;
            LevelBase = LevelController.Instance.LevelBase;
            delayCustomer = LevelBase.delayPerCustomer;
        }

        public void AddController() => MainController.Instance.AddController(this);

    }
}
