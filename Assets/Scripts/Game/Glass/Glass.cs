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

        private void Start() => glassState = GlassState.EMPTY;

        private void checkedMenu() => isValidMenu = ResourceManager.Instance.igrendientsToMenuChecker(igrendients, out getMenuState);

        int tap = 0;
        float interval = .5f;
        private void OnMouseDown()
        {
            tap++;

            if(tap == 1)
            {
                StartCoroutine(IDoubleClick());
            } else if(tap>1)
            {
                tap = 0;    // reset
                print("Double");
            }
            //boxCollider2D.enabled = false;      // Prevent brute force
            //if (isValidMenu
            //    && OrderController.Instance.isExistQueue(getMenuState, out targetBuyer)
            //    )
            //{
            //    targetBuyer.customerHandler.onServeMenu(getMenuState);

            //    GlassContainer.Instance.glassOnDestroy(glassRegistered);
            //    StartCoroutine(IDestroy());
            //    return;
            //}
            //boxCollider2D.enabled = true;
        }

        IEnumerator IDoubleClick()
        {
            yield return new WaitForSeconds(interval);
            if(tap == 1)
            {
                print("single");
            }
            tap = 0;
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


        #endregion
    }
}
