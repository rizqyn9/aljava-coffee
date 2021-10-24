using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game
{
    public static class ANIM
    {
        public static readonly string NGOMONG = "ngomong";
        public static readonly string MARAH = "marah";
        public static readonly string SENANG = "senang";
    }

    public class CustomerHandler : MonoBehaviour
    {
        public Transform spawnCharTransform;
        public GameObject bubbles;
        public SpriteRenderer[] menuSpawnRenderer;
        public SpriteRenderer singleMenuSpawnRenderer;
        public PatienceBar patienceBar;
        public enum EmotionalState
        {
            NGOMONG,
            IDDLE,
            MARAH
        }

        [Header("Debug")]
        [SerializeField] BuyerType buyerType;
        [SerializeField] BuyerPrototype buyerPrototype;
        [SerializeField] List<buyerOrderItemHandler> orderItemHandlers;
        [SerializeField] Transform destinationSeat;
        [SerializeField] Animator animator;
        [SerializeField] GameObject gameChar;
        //[SerializeField] EmotionalState EmotionalState;

        public void initBuyer(BuyerPrototype _buyerPrototype)
        {
            buyerPrototype = _buyerPrototype;
            buyerType = _buyerPrototype.buyerType;
            gameObject.name = _buyerPrototype.customerCode;

            gameChar = Instantiate(_buyerPrototype.buyerType.buyerPrefab, spawnCharTransform);
            animator = gameChar.GetComponentInChildren<Animator>();

            renderMenu();

            buyerPrototype.customerHandler = this;

            StartCoroutine(startCustomer());
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

            buyerOrderItemHandler itemHandler = new buyerOrderItemHandler()
            {
                itemGO = _renderer.gameObject,
                menu = _menuName
            };

            orderItemHandlers.Add(itemHandler);
        }

        /// <summary>
        /// Trigger when player success to serving a menu
        /// </summary>
        /// <param name="_menu"></param>
        public void onServeMenu(MenuType _menu)
        {
            orderItemHandlers.Find(val => val.menu == _menu).itemGO.SetActive(false);
        }

        /// <summary>
        /// Trigger when player success to serve all menus
        /// </summary>
        public void onMenusDone()
        {
            CustomerController.Instance.OnCustomerDone(buyerPrototype);

            RulesController.OnCustomerServed(buyerPrototype);

            Destroy(gameObject);
        }

        [SerializeField] float _direction;
        [SerializeField] float _duration;
        IEnumerator startCustomer()
        {
            _direction = buyerPrototype.spawnPos.x - buyerPrototype.seatPos.x;
            _direction = _direction < 0 ? _direction * -1 : _direction;
            _duration = _direction / 5;


            gameObject.transform.LeanMove(buyerPrototype.seatPos, _duration);
            yield return new WaitForSeconds(_duration);

            StartCoroutine(INgomong(true));

            OrderController.Instance.deliveryQueueMenu.Add(buyerPrototype);
            //yield return new WaitForSeconds(_duration/2);
            bubbles.SetActive(true);

            patienceBar.gameObject.SetActive(true);

            Vector2 defScale = bubbles.transform.localScale;
            bubbles.transform.localScale = Vector2.zero;
            bubbles.LeanScale(defScale, .5f).setEaseInBounce();

            yield return new WaitForSeconds(2);
            StartCoroutine(INgomong(false));
            patienceBar.StartBar(15f);
            yield break;

            // Animate when Customer already spawned
        }

        public IEnumerator INgomong(bool isActive)
        {
            animator.SetBool(ANIM.NGOMONG, isActive);
            yield return new WaitForSeconds(2);
            if(isActive) animator.SetBool(ANIM.NGOMONG, !isActive);
            yield break;
        }

        public IEnumerator IMarah()
        {
            animator.SetBool(ANIM.MARAH, true);
            yield return new WaitForSeconds(5f);
            animator.SetBool(ANIM.MARAH, false);
            yield break;
        }

    }
}
