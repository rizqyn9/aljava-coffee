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
        public GameObject winPrefab;
        public GameObject loosePrefab;

        [Header("Debug")]
        public string glassCode;
        public List<enumIgrendients> igrendients;
        public MenuType getMenuState;
        public bool isValidMenu = false;
        [SerializeField] SpriteRenderer debugIgrendientsGO;
        [SerializeField] enumIgrendients _lastIgrendients = enumIgrendients.NULL;
        [SerializeField] BuyerPrototype targetBuyer;
        [SerializeField] GlassRegistered glassRegistered;
        public enumIgrendients lastIgrendients
        {
            get => _lastIgrendients;
            set
            {
                checkedMenu();
                _lastIgrendients = value;
            }
        }
        [SerializeField] float doubleClickTimeLimit = 0.1f;

        [ContextMenu("Win")]
        public void winCondition()
        {
            Instantiate(winPrefab);
        }
        [ContextMenu("Loose")]
        public void looseCondition()
        {
            Instantiate(loosePrefab);
        }

        private void checkedMenu()
        {
            isValidMenu = ResourceManager.Instance.igrendientsToMenuChecker(igrendients, out getMenuState);
        }

        public List<GameObject> listIgrendientsGO = new List<GameObject>();
        public GameObject GO;
        private BoxCollider2D boxCollider2D;

        private void Awake()
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            StartCoroutine(InputListener());

            glassCode = generateUniqueCode();
            gameObject.name = glassCode;

            glassRegistered = new GlassRegistered() { glassCode = glassCode, glass = this };
            LevelManager.Instance.listGlassRegistered.Add(glassRegistered);
            GlassContainer.Instance.glassRegistereds.Add(glassRegistered);
        }

        private string generateUniqueCode() => $"--Glass-{GlassContainer.Instance.getCode()}";

        public void changeSpriteAddIgrendients(Color _color, List<enumIgrendients> _listIgrendients)
        {
            if (!debugIgrendientsGO)
            {
                debugIgrendientsGO = Instantiate(baseIgrendientsGO, igrendientTransform).GetComponent<SpriteRenderer>();
            }
            igrendients.AddRange(_listIgrendients);
            setLastIgrendients(_listIgrendients[_listIgrendients.Count - 1]);
            debugIgrendientsGO.color = new Color(_color.r, _color.g, _color.b, 1);
        }

        void setLastIgrendients(enumIgrendients _lastIgrendients) => lastIgrendients = _lastIgrendients;

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

        private void OnMouseDown()
        {
            if (isValidMenu && MainController.Instance.isExistQueue(getMenuState.menuListName, out targetBuyer))
            {
                Debug.Log("Find customer menu");
                targetBuyer.customerHandler.onServeMenu(getMenuState.menuListName);

                GlassContainer.Instance.canSpawn = true;
                GlassContainer.Instance.glassOnDestroy(glassRegistered);
                Destroy(gameObject);
            }
            
        }

        private void SingleClick()
        {
        }

        private void DoubleClick()
        {
            Debug.Log("Double Click");
        }
    }
}
