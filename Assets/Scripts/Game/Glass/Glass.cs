using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Glass : MonoBehaviour
    {
        [Header("Properties")]
        public Transform igrendientTransform;
        public GameObject baseIgrendientsGO;

        [Header("Debug")]
        public GlassState glassState;
        public List<MachineIgrendient> igrendients = new List<MachineIgrendient>();
        public MenuType getMenuState;
        public bool isValidMenu = false;
        public GlassRegistered glassRegistered;
        [SerializeField] SpriteRenderer igrendientRenderer;
        [SerializeField] BuyerPrototype targetBuyer;
        [SerializeField] MachineIgrendient _lastIgrendients = MachineIgrendient.NULL;
        public MachineIgrendient lastIgrendients
        {
            get => _lastIgrendients;
            set
            {
                checkedMenu();
                _lastIgrendients = value;
            }
        }
        private BoxCollider2D boxCollider2D;
        [SerializeField] float doubleClickTimeLimit = 0.1f;

        private void Awake()
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            StartCoroutine(InputListener());

            glassState = GlassState.EMPTY;
        }

        private void checkedMenu() => isValidMenu = ResourceManager.Instance.igrendientsToMenuChecker(igrendients, out getMenuState);

        private void OnMouseDown()
        {
            if (isValidMenu && MainController.Instance.isExistQueue(getMenuState, out targetBuyer))
            {
                Debug.Log("Find customer menu");
                targetBuyer.customerHandler.onServeMenu(getMenuState);

                GlassContainer.Instance.glassOnDestroy(glassRegistered);
                StartCoroutine(IDestroy());
            }

        }

        IEnumerator IDestroy()
        {
            gameObject.LeanScale(Vector2.zero, .5f);
            yield return new WaitForSeconds(1f);

            GlassContainer.Instance.reqGlassSpawn(glassRegistered.seatIndex);

            Destroy(gameObject);
            yield break;
        }

        #region Depends
        /// <summary>
        /// Add Igrendients and rendering Sprite result
        /// _igrendiets automatically set as lastIgrendients
        /// </summary>
        /// <param name="_color"></param>
        /// <param name="_igrendients (optional)"></param>
        /// <param name="_multipleIgrendients (optional)"></param>
        public void changeSpriteAddIgrendients(Color _color, MachineIgrendient _igrendients = MachineIgrendient.NULL, List<MachineIgrendient> _multipleIgrendients = null)
        {
            if (!igrendientRenderer) igrendientRenderer = Instantiate(baseIgrendientsGO, igrendientTransform).GetComponent<SpriteRenderer>();
            if (_multipleIgrendients != null)
            {
                igrendients.AddRange(_multipleIgrendients);
                lastIgrendients = _multipleIgrendients[_multipleIgrendients.Count - 1];
            }
            if(_igrendients != MachineIgrendient.NULL)
            {
                igrendients.Add(_igrendients);
                lastIgrendients = _igrendients;
            }

            igrendientRenderer.color = new Color(_color.r, _color.g, _color.b, 1);
        }

        public void process()
        {
            StartCoroutine(IProcess());
        }

        IEnumerator IProcess()
        {
            glassState = GlassState.PROCESS;
            animateGlass();

            yield return new WaitForSeconds(1);
            glassState = GlassState.FILLED;
        }

        private void animateGlass()=> LeanTween.scale(gameObject, new Vector2(2.5f, 2.5f), .3f).setEase(LeanTweenType.easeInOutCirc).setLoopPingPong(1);

        private IEnumerator InputListener()
        {
            while (enabled)
            { 
                if (boxCollider2D.isTrigger)
                    yield return ClickEvent();

                yield return null;
            }
        }

        private IEnumerator ClickEvent()
        {
            yield return new WaitForEndOfFrame();

            float count = 0f;
            while (count < doubleClickTimeLimit)
            {
                if (boxCollider2D.isTrigger)
                {
                    DoubleClick();
                    yield break;
                }
                count += Time.deltaTime;
                yield return null;
            }
            SingleClick();
        }

        private void SingleClick()
        {
        }

        private void DoubleClick()
        {
            Debug.Log("Double Click");
        }

        #endregion
    }
}
