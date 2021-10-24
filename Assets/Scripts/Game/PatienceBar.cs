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
        public CustomerHandler customerHandler;

        public void init(CustomerHandler _customerHandler)
        {
            gameObject.SetActive(true);
            gameObject.LeanAlpha(0, 0);
            customerHandler = _customerHandler;
        }

        internal void startBar(float _duration)
        {
            gameObject.LeanAlpha(1, .5f);
            duration = _duration;

            StartCoroutine(IStartBar());
        }

        [SerializeField] float _valPatience;
        [SerializeField] bool canAngry = true;
        IEnumerator IStartBar()
        {
            yield return 0;
            filled.LeanMoveLocalY(-1f, duration).setOnUpdate((float val) =>
            {
                if (val > .8f && canAngry)
                {
                    canAngry = false;
                    customerHandler.onPatienceAngry();
                }
            }).setOnComplete(() =>
            {
                customerHandler.onPatienceRunOut();
            });
            yield break;            
        }
    }
}
