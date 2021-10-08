using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Game
{
    public class BarMachine : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] Image bar;

        [Header("Debug")]
        public float time;

        public void StartBar() => StartCoroutine(barStart());

        IEnumerator barStart()
        {
            LeanTween.value(0, 100, time).setOnUpdate((float val) =>
             {
                 bar.fillAmount = val / 100;
             });

            yield return new WaitForSeconds(time);

            bar.fillAmount = 0;

            yield break;

        }
    }
}
