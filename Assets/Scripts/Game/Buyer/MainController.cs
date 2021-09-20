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

    public class MainController : Singleton<MainController>
    {
        [Header("Properties")]
        public transformSeatData[] seatDataTransform;
        public Transform[] startSpawnTransform;
        public int delay = 10;
        public GameObject tempCustomer;
        public int targetCustomer = 5;
        public GameObject winPrefab;

        [Header("Debug")]
        public List<BuyerPrototype> deliveryQueueMenu = new List<BuyerPrototype>();
        public int maxSlotOrder;
        public bool isDeliveryFull = false;
        public bool isGameFinished = false;
        public bool canCreateCustomer = false;
        public List<int> freeSeatDataIndex = new List<int>();
        [SerializeField] int buyerAssetCount;
        [SerializeField] int menuAssetCount;
        [SerializeField] int customerCounter = 1;
        [SerializeField] bool isGameEnd = false;

        public void Start()
        {
            Application.targetFrameRate = 60; // Optional platform

            buyerAssetCount = ResourceManager.Instance.BuyerTypes.Count;
            menuAssetCount = ResourceManager.Instance.MenuTypes.Count;
            maxSlotOrder = seatDataTransform.Length;
            StartCoroutine(onStart());
        }

        IEnumerator onStart()
        {
            yield return new WaitForSeconds(1);
            canCreateCustomer = true;
        }
        private void Update()
        {
            isGameFinished = deliveryQueueMenu.Count == targetCustomer;
            if (isGameFinished && !isGameEnd)
            {
                monitorAvaibleSeat();
                if(freeSeatDataIndex.Count == seatDataTransform.Length)
                {
                    Debug.Log("asdsad");
                    StartCoroutine(gameFinished());
                    return;

                }
            }

            if (deliveryQueueMenu.Count >= maxSlotOrder)
                isDeliveryFull = true;
            else
                isDeliveryFull = false;

            if(canCreateCustomer && !isGameFinished)
            {
                if (monitorAvaibleSeat() && !isGameFinished)
                {
                    createNewCustomer(freeSeatDataIndex[Random.Range(0, freeSeatDataIndex.Count)]);
                }
                else
                {
                    return;
                }
            }
        }

        IEnumerator gameFinished()
        {
            new WaitForSeconds(4);
            winPrefab.SetActive(true);
            isGameEnd = true;
            yield break;
        }

        private void createNewCustomer(int _seatIndex)
        {
            canCreateCustomer = false;
            StartCoroutine(reactiveCustomerCreation());
            seatDataTransform[_seatIndex].isSeatAvaible = false;

            GameObject GO = Instantiate(tempCustomer, startSpawnTransform[Random.Range(0,2)]);
            CustomerHandler customer = GO.GetComponent<CustomerHandler>();

            BuyerPrototype buyerPrototype = new BuyerPrototype();
            buyerPrototype.buyerType = ResourceManager.Instance.BuyerTypes[Random.Range(0, buyerAssetCount)];
            buyerPrototype.customerCode = $"Customer-{customerCounter++}";
            buyerPrototype.seatIndex = _seatIndex;
            buyerPrototype.menuListNames = generateMenu(Random.Range(1,2));

            // reference buyer
            customer.initBuyer(buyerPrototype);
        }

        public bool findMenu(menuListName _menu, out CustomerHandler _customerHandler)
        {
            bool res = false;
            _customerHandler = null;
            for(int i= 0; i< deliveryQueueMenu.Count; i++)
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

        List<menuListName> generateMenu(int _total)
        {
            List<menuListName> res = new List<menuListName>();
            for(int i =0; i< _total; i++)
            {
                res.Add(ResourceManager.Instance.MenuTypes[Random.Range(0, menuAssetCount)].menuListName);
            }
            return res;
        }

        IEnumerator reactiveCustomerCreation()
        {
            yield return new WaitForSeconds(delay);
            canCreateCustomer = true;
            yield break;
        }

        bool monitorAvaibleSeat()
        {
            freeSeatDataIndex = new List<int>();
            for(int i = 0; i < seatDataTransform.Length; i++)
            {
                if (seatDataTransform[i].isSeatAvaible) freeSeatDataIndex.Add(i);
            }
            if (freeSeatDataIndex.Count == 0) return false;
            else return true;
        }

        public bool isExistQueue(menuListName _menuName, out BuyerPrototype _buyerPrototype)
        {
            _buyerPrototype = new BuyerPrototype();
            try
            {
                for(int i =0; i<deliveryQueueMenu.Count; i++)
                {
                    if (deliveryQueueMenu[i].menuListNames.Contains(_menuName))
                    {
                        _buyerPrototype = deliveryQueueMenu[i];
                        _buyerPrototype.menuListNames.Remove(_menuName);
                        if(_buyerPrototype.menuListNames.Count < 1)
                        {
                            Debug.Log("Clear");
                            _buyerPrototype.customerHandler.onMenusDone();
                        }
                        clearQueue(deliveryQueueMenu[i]);
                        return true;
                    }
                }
                throw new System.Exception();
            } catch
            {
                return false;
            }
        }

        private void clearQueue(BuyerPrototype _buyerPrototype)
        {
            
        }
    }
}
