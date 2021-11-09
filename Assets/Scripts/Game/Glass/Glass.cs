using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Glass : MonoBehaviour
    {
        [Header("Properties")]
        public SpriteRenderer baseSprite;
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
        float interval = .3f;
        private void OnMouseDown()
        {
            tap++;

            if(tap == 1)
            {
                StartCoroutine(IDoubleClick());
            } else if(tap>1)
            {
                tap = 0;    // reset
                OnDoubleClick();
            }
        }

        private void OnDoubleClick()
        {
            if (lastIgrendients == MachineIgrendient.NULL) return;
            OnTrash();   
        }

        private void OnTrash()
        {
            EnvController.Instance.OnTrash(this);
        }

        private void OnSingleClick()
        {
            boxCollider2D.enabled = false;      // Prevent brute force
            if (isValidMenu
                && OrderController.Instance.isExistQueue(getMenuState, out targetBuyer)
                )
            {
                targetBuyer.customerHandler.onServeMenu(getMenuState);
                destroyGlass();
                GlassContainer.Instance.glassOnDestroy(glassRegistered);
                return;
            }
            boxCollider2D.enabled = true;
        }


        IEnumerator IDoubleClick()
        {
            yield return new WaitForSeconds(interval);
            if(tap == 1)
            {
                OnSingleClick();
            }
            tap = 0;
        }

        public void destroyGlass() => StartCoroutine(IDestroy());
        IEnumerator IDestroy()
        {
            gameObject.LeanScale(Vector2.zero, .5f);
            yield return new WaitForSeconds(1f);

            reqSpawnGlassContainer();

            Destroy(gameObject);
            yield break;
        }

        void reqSpawnGlassContainer() => GlassContainer.Instance.reqGlassSpawn(glassRegistered.seatIndex);


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
            baseChange(_igrendients, _multipleIgrendients, MethodChange.CHANGE_CHILD);

            igrendientRenderer.color = new Color(_color.r, _color.g, _color.b, 1);
        }

        /// <summary>
        /// Add Igrendients and rendering Sprite result
        /// _igrendiets automatically set as lastIgrendients
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="_igrendients"></param>
        /// <param name="_multipleIgrendients"></param>
        public void changeSpriteAddIgrendients(Sprite _sprite, MachineIgrendient _igrendients = MachineIgrendient.NULL, List<MachineIgrendient> _multipleIgrendients = null)
        {
            baseChange(_igrendients, _multipleIgrendients, MethodChange.CHANGE_BASE);

            baseSprite.sprite = _sprite;
        }

        enum MethodChange
        {
            CHANGE_BASE,
            CHANGE_CHILD
        }
        private void baseChange(MachineIgrendient _igrendients, List<MachineIgrendient> _multipleIgrendients, MethodChange methodChange)
        {
            if (!baseSprite.isVisible)
            {
                baseSprite.enabled = true;
            }
            if(methodChange == MethodChange.CHANGE_BASE)
            {
                if (igrendientRenderer) igrendientRenderer.enabled = false;
            } else
            {
                if (!igrendientRenderer) igrendientRenderer = Instantiate(baseIgrendientsGO, igrendientTransform).GetComponent<SpriteRenderer>();
            }
            if (_multipleIgrendients != null)
            {
                igrendients.AddRange(_multipleIgrendients);
                lastIgrendients = _multipleIgrendients[_multipleIgrendients.Count - 1];
            }
            if (_igrendients != MachineIgrendient.NULL)
            {
                igrendients.Add(_igrendients);
                lastIgrendients = _igrendients;
            }
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

        private void animateGlass()=> LeanTween.scale(gameObject, new Vector2(.27f, .27f), .1f).setEase(LeanTweenType.easeInOutCirc).setLoopPingPong(1);


        #endregion
    }
}
