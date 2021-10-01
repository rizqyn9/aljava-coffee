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
        [SerializeField] GameState _gameState;
        public GameState GameState
        {
            get => _gameState;
            set => _gameState = value;
        }

        public void OnGameStateChanged() => _gameState = MainController.Instance.GameState;

        internal void Init()
        {
            MainController.Instance.RegistGameState(this);

        }
        private void Start()
        {
            CustomerController = CustomerController.Instance;
            print("Order Controller");
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
                            Debug.Log("Clear");
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


    }

}
