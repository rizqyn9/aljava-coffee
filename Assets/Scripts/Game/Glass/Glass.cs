using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Glass : MonoBehaviour
    {
        [Header("Properties")]
        public string glassCode;
        public Transform igrendientTransform;
        public List<enumIgrendients> igrendients;

        [Header("Debug")]
        public MenuType getMenuState;
        public bool isValidMenu = false;
        private enumIgrendients _lastIgrendients = enumIgrendients.NULL;
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


        private void checkedMenu()
        {
            isValidMenu = ResourceManager.Instance.igrendientsToMenuChecker(igrendients, getMenuState);
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
            GlassRegistered glassRegistered = new GlassRegistered() { glassCode = glassCode, glass = this };
            LevelManager.Instance.listGlassRegistered.Add(glassRegistered);
            GlassContainer.Instance.glassRegistereds.Add(glassRegistered);
        }

        private string generateUniqueCode() => $"--Glass{GlassContainer.Instance.getCode()}";

        public void addIgredients(GameObject _prefab, enumIgrendients _igrendient)
        {
            GO = Instantiate(_prefab, igrendientTransform);
            listIgrendientsGO.Add(GO);
            lastIgrendients = _igrendient;
            igrendients.Add(_igrendient);
        }

        public void addMultipleIgrendients(GameObject _prefab, List<enumIgrendients> _igrendients, enumIgrendients _setLastIgrendient)
        {
            GO = Instantiate(_prefab, igrendientTransform);
            listIgrendientsGO.Add(GO);
            lastIgrendients = _setLastIgrendient;
            foreach(enumIgrendients _igrendient in _igrendients)
            {
                igrendients.Add(_igrendient);
            }
        }

        public void changeSpriteIgrendients(Sprite _sprite, enumIgrendients _igrendients)
        {
            SpriteRenderer renderer = GO.GetComponent<SpriteRenderer>();
            igrendients.Add(_igrendients);
            lastIgrendients = _igrendients;
            renderer.sprite = _sprite;
        }

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
            if (isValidMenu)
            {
                Debug.Log("Find customer menu");
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
