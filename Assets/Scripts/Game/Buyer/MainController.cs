using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        [Header("Debug")]
        public List<BuyerPrototype> deliveryQueueMenu = new List<BuyerPrototype>();
        public int maxSlotOrder;
        public bool isDeliveryFull = false;
        public bool isGameStarted = false;
        public bool isGameFinished = false;
        public bool isGameTimeOut = false;
        public bool canCreateCustomer = false;
        public List<int> freeSeatDataIndex = new List<int>();
        [SerializeField] ResourceData ResourceData;
        [SerializeField] int customerCounter = 1;
        [SerializeField] bool isGameEnd = false;
        [SerializeField] List<Machine> Machines = new List<Machine>();

        public void Start()
        {
            Application.targetFrameRate = 60; // Optional platform

            ResourceData = ResourceManager.Instance.resourceData;

            maxSlotOrder = seatDataTransform.Length;

            Machines = FindObjectsOfType<Machine>().ToList();

            initMachine();

            GameUIController.Instance.timerIsRunning = true;
            StartCoroutine(onStart());
        }

        private void initMachine()
        {
            foreach(Machine _ in Machines)
            {
                _.isGameStarted = true;
                _.startMachine();
            }
        }

        IEnumerator onStart()
        {
            yield return new WaitForSeconds(1);

            isGameStarted = true;
            canCreateCustomer = true;
        }

        /// <summary>
        /// finding Machine target, with out Machine
        /// </summary>
        /// <param name="_machine"></param>
        /// <param name="_">out, bring out Machine class</param>
        /// <returns></returns>
        public bool isMachineAvaible(MachineType _machine, out Machine _)
        {
            _ = Machines.Find(val => val.machineType == _machine && val.MachineState == MachineState.ON_IDDLE);
            return _;
        }

        private void Update()
        {
            if (isGameTimeOut)
            {
                StartCoroutine(gameFinished());
                return;
            }
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
            buyerPrototype.buyerType = ResourceManager.Instance.BuyerTypes[Random.Range(0, ResourceData.buyerTypeCount)];
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
                res.Add(ResourceManager.Instance.MenuTypes[Random.Range(0, ResourceData.menuTypeCount)].menuListName);
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
