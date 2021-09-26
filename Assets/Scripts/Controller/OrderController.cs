using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    [RequireComponent(typeof(CustomerController))]
    public class OrderController : Singleton<OrderController>
    {
        public List<BuyerPrototype> deliveryQueueMenu = new List<BuyerPrototype>();
        public CustomerController CustomerController = null;

        private void Start()
        {
            CustomerController = CustomerController.Instance;
            print("Order Controller");
        }
    }
}
