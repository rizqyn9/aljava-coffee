using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public struct BuyerPrototype
    {
        public string customerCode;
        public BuyerType buyerType;
        public buyerState buyerState;
        public Transform seatDestination;
        public List<menuListName> menuListNames;
        public CustomerHandler customerHandler;
    }

    public class CustomerHandler : MonoBehaviour
    {
        public Transform spawnCharTransform;
        public GameObject bubbles;

        [Header("Debug")]
        [SerializeField] BuyerPrototype buyerPrototype;
        public Transform destinationSeat;
        [SerializeField] int seatIndex;

        public void initBuyer(BuyerPrototype _buyerPrototype)
        {
            buyerPrototype = _buyerPrototype;
            gameObject.name = _buyerPrototype.customerCode;
            Instantiate(_buyerPrototype.buyerType.buyerPrefab, spawnCharTransform);
            buyerPrototype.customerHandler = this;

            StartCoroutine(startCustomer(1));
        }

        IEnumerator startCustomer(int _duration)
        {
            gameObject.transform.LeanMove(buyerPrototype.seatDestination.position, _duration);
            new WaitForSeconds(_duration);
            MainController.Instance.deliveryQueueMenu.Add(buyerPrototype);
            yield break;

            // Animate when Customer already spawned
        }

    }
}
