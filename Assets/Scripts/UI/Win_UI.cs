using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private void Start()
        {
            //testAnim(3);
            canvasGroup.alpha = 0;
            mainContainer.localScale = Vector2.zero;

            init();
        }

        public void init()
        {
            print("Win");
            setUpComponent();
            testAnim(3);
        }

        private void testAnim(int starSpawn)
        {
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

                yield return new WaitForSeconds(1);
                total--;
            }
            yield break;
        }

        public void animStar(int target)
        {
            LeanTween.scale(stars[target-1].go, new Vector2(1f, 1f), 1f);
        }
    }
}
