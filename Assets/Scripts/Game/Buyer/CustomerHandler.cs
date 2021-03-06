using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            bubbles.SetActive(false);

            renderMenu();
            patienceBar.init(this);

            buyerPrototype.customerHandler = this;

            StartCoroutine(IWalkSeat());
        }


        #region Menu Handler
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

        #endregion

        #region Event Customer

        public void onSeat()
        {
            StartCoroutine(IOnSeat());
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
            onLeaveSeat(false);
        }

        public void onPatienceAngry()
        {
            StartCoroutine(IMarah());
        }

        public void onPatienceRunOut()
        {
            onLeaveSeat(true);
        }

        public void onLeaveSeat(bool isOnAngry)
        {
            OrderController.Instance.OnCustLeave(buyerPrototype);
            CustomerController.Instance.customerPresence(
                success: isOnAngry ? 0 : 1,
                runOut: isOnAngry ? 1 : 0
                );
            CustomerController.Instance.OnLeave(buyerPrototype, isOnAngry);
            StartCoroutine(ILeaveSeat());
        }

        #endregion

        #region IEnumerator Controller
        [SerializeField] float _direction;
        [SerializeField] float _duration;

        IEnumerator IWalkSeat()
        {
            // Walk to seat position
            _direction = buyerPrototype.spawnPos.x - buyerPrototype.seatPos.x;
            _direction = _direction < 0 ? _direction * -1 : _direction;
            _duration = _direction / 5;

            gameObject.transform.LeanMove(buyerPrototype.seatPos, _duration).setOnComplete(() => {
                onSeat();
            });

            yield break;
        }

        IEnumerator IOnSeat()
        {
            StartCoroutine(INgomong(true));
            OrderController.Instance.deliveryQueueMenu.Add(buyerPrototype);

            Vector2 defScale = bubbles.transform.localScale;
            bubbles.transform.localScale = Vector2.zero;
            bubbles.SetActive(true);
            bubbles.LeanScale(defScale, .5f).setEaseInBounce();

            patienceBar.startBar(buyerPrototype.buyerType.patienceDuration);
            StartCoroutine(INgomong(false));
            yield break;
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

        public IEnumerator ILeaveSeat()
        {
            bubbles.LeanScale(Vector2.zero, .5f).setEaseInBounce();
            yield return new WaitForSeconds(.5f);

            gameObject.LeanMove(buyerPrototype.spawnPos, _duration).setOnComplete(() =>
            {
                Destroy(gameObject);
            });

            yield break;
        }

        #endregion
    }
}
