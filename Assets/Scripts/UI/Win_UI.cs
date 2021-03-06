using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Win_UI : MonoBehaviour
    {
        [System.Serializable]
        public struct goStar
        {
            public GameObject go;
            public Vector3 initialPos;
        }

        [Header("Properties")]
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] RectTransform mainContainer;
        public Vector2 offset;
        public List<goStar> stars;
        public List<Button> buttons;

        private void Start()
        {
            //testAnim(3);
            canvasGroup.alpha = 0;
            mainContainer.localScale = Vector2.zero;

            buttons.ForEach(val => val.gameObject.SetActive(false));

            //init();
        }

        public void init()
        {
            setUpComponent();
            testAnim(MainController.RulesController.starTotal);
        }

        private void testAnim(int starSpawn)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;

            LeanTween.alphaCanvas(canvasGroup, 1f, .6f);
            LeanTween.scale(mainContainer, new Vector2(1f, 1f), .6f).setEase(LeanTweenType.easeOutBack);

            StartCoroutine(ISpawnStars(starSpawn));
        }

        private void setUpComponent()
        {
            stars.ForEach(val =>
            {
                val.go.GetComponent<RectTransform>().localScale = Vector2.zero;
            });
        }

        IEnumerator ISpawnStars(int total)
        {
            //bool onWait = false;
            while (total > 0 )
            {
                //onWait = true;

                animStar(total);

                yield return new WaitForSeconds(.5f);
                total--;
            }
            yield return new WaitForSeconds(1f);
            buttons.ForEach(val => val.gameObject.SetActive(true));
            yield break;
        }

        public void animStar(int target)
        {
            LeanTween.scale(stars[target - 1].go, new Vector2(1f, 1f), 1f).setEaseInBounce();
        }

        public void Btn_Restart()
        {
            GameManager.Instance.loadLevel(MainController.Instance.levelBase.level);
        }

        public void Btn_Next()
        {
            GameManager.Instance.loadLevel(MainController.Instance.levelBase.level + 1);
        }

        public void Btn_Home()
        {
            GameManager.Instance.backToHome();
        }
    }
}
