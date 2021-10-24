using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    [RequireComponent(typeof(CustomerController))]
    public class OrderController : Singleton<OrderController>, IGameState
    {
        public List<BuyerPrototype> deliveryQueueMenu = new List<BuyerPrototype>();
        public CustomerController CustomerController = null;

        [Header("Debug")]
        [SerializeField] GameState gameState;

        #region GAME STATE
        private void OnEnable() => MainController.OnGameStateChanged += GameStateHandler;
        private void OnDisable() => MainController.OnGameStateChanged += GameStateHandler;

        public void GameStateHandler(GameState _gameState)
        {
            gameState = _gameState;
            GameStateController.UpdateGameState(this, gameState);
        }

        internal void Init()
        {
            deliveryQueueMenu = new List<BuyerPrototype>();
        }

        public void OnGameIddle() { }

        public void OnGameBeforeStart() { }

        public void OnGameStart() { }

        public void OnGamePause() { }

        public void OnGameClearance() { }

        public void OnGameFinish() { }

        public void OnGameInit() { }
        #endregion

        protected void Awake()
        {
            CustomerController = CustomerController.Instance;
        }

        public bool findMenu(MenuType _menu, out CustomerHandler _customerHandler)
        {
            bool res = false;
            _customerHandler = null;
            for (int i = 0; i < deliveryQueueMenu.Count; i++)
            {
                res = deliveryQueueMenu[i].menuListNames.Contains(_menu);
                if (res)
                {
                    Debug.Log("Found Menu in Queue");
                    _customerHandler = deliveryQueueMenu[i].customerHandler;
                    break;
                }
            }
            return res;
        }

        public bool isExistQueue(MenuType _menuName, out BuyerPrototype _buyerPrototype)
        {
            _buyerPrototype = new BuyerPrototype();
            try
            {
                for (int i = 0; i < deliveryQueueMenu.Count; i++)
                {
                    if (deliveryQueueMenu[i].menuListNames.Contains(_menuName))
                    {
                        _buyerPrototype = deliveryQueueMenu[i];
                        _buyerPrototype.menuListNames.Remove(_menuName);
                        if (_buyerPrototype.menuListNames.Count < 1)
                        {
                            _buyerPrototype.customerHandler.onMenusDone();
                        }
                        clearQueue(deliveryQueueMenu[i]);
                        return true;
                    }
                }
                throw new System.Exception();
            }
            catch
            {
                return false;
            }
        }

        private void clearQueue(BuyerPrototype _buyerPrototype) { }

        public GameObject GetGameObject() => gameObject;
    }
}
