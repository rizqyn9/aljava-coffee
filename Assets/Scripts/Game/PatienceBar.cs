using System.Collections;
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

        internal void StartBar(float _duration)
        {
            duration = _duration;
            CustomerHandler = GetComponentInParent<CustomerHandler>();
            StartCoroutine(IStartBar());
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
    }
}
