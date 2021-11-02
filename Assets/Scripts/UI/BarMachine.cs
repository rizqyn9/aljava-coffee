using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Game
{
    public class BarMachine : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] Image bar;
        [SerializeField] GameObject checkListGO;

        [Header("Debug")]
        [SerializeField] float time;
        [SerializeField] Machine machine;
        [SerializeField] bool isActive;
        public enum BarType
        {
            OVERCOOK,
            DEFAULT
        }

        public void init(Machine _machine)
        {
            machine = _machine;
            time = machine.machineProperties.processDuration;
        }

        public void runProgress(BarType _barType)
        {
            //Debug.LogWarning("active progress");
            //if (isActive) return;
            StartCoroutine(IStart(_barType));
        }

        IEnumerator IStart(BarType _barType)
        {
            isActive = true;

            bar.fillAmount = 0;
            gameObject.LeanAlpha(1, .5f);
            gameObject.LeanScale(new Vector2(1, 1), .5f).setEaseInBounce();
            yield return new WaitForSeconds(.5f);

            if (_barType == BarType.OVERCOOK)
            {
                bar.color = Color.red;
            } else
            {
                bar.color = Color.black;
            }

            LeanTween.value(0, 100, time).setOnUpdate((float val) =>
            {
                bar.fillAmount = val / 100;
            }).setOnComplete(()=>
            {
                if (!machine.isUseBarCapacity)
                {
                    if (machine.isUseOverCook)
                    {
                        if(_barType == BarType.OVERCOOK)
                        {
                            machine.initRepair();
                            return;
                        }
                        print("Instance Overcook");
                        resetProgress();
                        machine.initOverCook();
                    } else
                    {
                        hadleCheckList(true);
                    }
                } else
                {
                    resetProgress();
                }
                machine.barMachineDone();
            });

            yield break;
        }

        void hadleCheckList(bool isActive)
        {
            checkListGO.SetActive(isActive);
        }

        public void resetProgress()
        {
            //Debug.LogWarning("REST");

            isActive = false;

            hadleCheckList(false);

            gameObject.LeanAlpha(0, .3f);
            transform.LeanScale(Vector2.zero, .3f);
        }

        private void OnEnable()
        {
            gameObject.LeanAlpha(0, 0);
            transform.LeanScale(Vector2.zero, 0);
        }
    }
}
