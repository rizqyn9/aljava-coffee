using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PatienceBar : MonoBehaviour
    {
        public GameObject filled;
        public bool started = false;
        public float valState = 10;
        public float duration;
        public CustomerHandler CustomerHandler;

        [ContextMenu("set")]
        public void setBar()
        {
            //filled.LeanScaleY(0, 3f);
            filled.LeanMoveLocalY(-1f, 3f);
        }

        internal void StartBar(float _duration)
        {
            duration = _duration;
            CustomerHandler = GetComponentInParent<CustomerHandler>();
            StartCoroutine(IStartBar());
            //StartCoroutine(IRunBar());

            //LeanTween.value(out )
        }

        IEnumerator IStartBar()
        {
            yield return 0;
            filled.LeanMoveLocalY(-1f, duration).setOnComplete(() =>
            {
                CustomerHandler.onMenusDone();
            });
            yield break;            
        }

        public float val = 0;
        private void Update()
        {
            if (filled.transform.hasChanged)
            {
                val = filled.transform.position.y;
                if(filled.transform.position.y == 0)
                {
                    print("marah");
                    CustomerHandler.StartCoroutine(CustomerHandler.IMarah());
                }
            }
        }

        [ContextMenu("test")]
        public void Test()
        {
            //StartCoroutine(IRunBar());
        }

        //IEnumerator IRunBar()
        //{
        //    filled.LeanMoveLocalY(-1f, duration);
        //    while (valState < float.MinValue)
        //    {
        //        filled.transform.pos
        //    }
        //}
    }
}
