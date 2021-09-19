using System;
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

    [System.Serializable]
    public struct buyerOrderItemHandler
    {
        public menuListName menu;
        public GameObject itemGO;
    }

    public class CustomerHandler : MonoBehaviour
    {
        public Transform spawnCharTransform;
        public GameObject bubbles;
        public SpriteRenderer[] menuSpawnRenderer;
        public SpriteRenderer singleMenuSpawnRenderer;

        [Header("Debug")]
        [SerializeField] BuyerPrototype buyerPrototype;
        [SerializeField] List<buyerOrderItemHandler> orderItemHandlers;
        [SerializeField] Transform destinationSeat;

        public void initBuyer(BuyerPrototype _buyerPrototype)
        {
            buyerPrototype = _buyerPrototype;
            gameObject.name = _buyerPrototype.customerCode;

            GameObject charSpawn = Instantiate(_buyerPrototype.buyerType.buyerPrefab, spawnCharTransform);

            renderMenu();

            buyerPrototype.customerHandler = this;

            StartCoroutine(startCustomer(1));
        }

        private void renderMenu()
        {
            if(buyerPrototype.menuListNames.Count == 1)
            {
                createMenuhandler(singleMenuSpawnRenderer, buyerPrototype.menuListNames[0]);
            } else
            {
                for(int i =0; i< buyerPrototype.menuListNames.Count; i++)
                {
                    createMenuhandler(menuSpawnRenderer[i], buyerPrototype.menuListNames[i]);
                }
            }
        }

        void createMenuhandler(SpriteRenderer _renderer, menuListName _menuName)
        {
            _renderer.sprite = ResourceManager.Instance.MenuTypes.Find(val => val.menuListName == _menuName).menuSprite;
            _renderer.enabled = true;

            buyerOrderItemHandler itemHandler = new buyerOrderItemHandler();
            itemHandler.itemGO = _renderer.gameObject;
            itemHandler.menu = _menuName;

            orderItemHandlers.Add(itemHandler);
        }

        public void onServeMenu(menuListName _menu)
        {
            orderItemHandlers.Find(val => val.menu == _menu).itemGO.SetActive(false);
        }

        IEnumerator startCustomer(int _duration)
        {
            gameObject.transform.LeanMove(buyerPrototype.seatDestination.position, _duration);
            yield return new WaitForSeconds(_duration);
            MainController.Instance.deliveryQueueMenu.Add(buyerPrototype);
            yield return new WaitForSeconds(_duration/2);
            bubbles.SetActive(true);
            yield break;

            // Animate when Customer already spawned
        }

    }
}
