using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
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

        void createMenuhandler(SpriteRenderer _renderer, MenuType _menuName)
        {
            _renderer.sprite = _menuName.menuSprite;
            _renderer.enabled = true;

            buyerOrderItemHandler itemHandler = new buyerOrderItemHandler();
            itemHandler.itemGO = _renderer.gameObject;
            itemHandler.menu = _menuName;

            orderItemHandlers.Add(itemHandler);
        }

        public void onServeMenu(MenuType _menu)
        {
            orderItemHandlers.Find(val => val.menu == _menu).itemGO.SetActive(false);
        }

        public void onMenusDone()
        {
            MainController.Instance.seatDataTransform[buyerPrototype.seatIndex].isSeatAvaible = true;
            GameUIController.Instance.asOrderCount();
            Destroy(gameObject);
        }

        IEnumerator startCustomer(int _duration)
        {
            gameObject.transform.LeanMove(MainController.Instance.seatDataTransform[buyerPrototype.seatIndex].transform.position, _duration).setEaseInBounce();
            yield return new WaitForSeconds(_duration);
            MainController.Instance.deliveryQueueMenu.Add(buyerPrototype);
            yield return new WaitForSeconds(_duration/2);
            bubbles.SetActive(true);

            Vector2 defScale = bubbles.transform.localScale;
            bubbles.transform.localScale = Vector2.zero;
            bubbles.LeanScale(defScale, .5f).setEaseInBounce();
            yield break;

            // Animate when Customer already spawned
        }

    }
}
