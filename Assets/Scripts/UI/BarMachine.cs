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
        [SerializeField] bool _isActive;
        public bool isActive
        {
            get => _isActive;
            set
            {
                if (value)
                {
                    runProgress();
                } else
                {
                    resetProgress();
                }
            }
        }

        public void init(Machine _machine)
        {
            machine = _machine;
            time = machine.MachineData.durationProcess;
        }

        private void runProgress()
        {
            StartCoroutine(IStart());
        }

        IEnumerator IStart()
        {
            bar.fillAmount = 0;
            gameObject.LeanAlpha(1, .5f);
            gameObject.LeanScale(new Vector2(1, 1), .5f).setEaseInBounce();
            yield return new WaitForSeconds(.5f);

            LeanTween.value(0, 100, time).setOnUpdate((float val) =>
            {
                bar.fillAmount = val / 100;
            }).setOnComplete(()=>
            {
                if (!machine.useCapacityComp)
                {
                    hadleCheckList(true);
                }
                machine.barMachineDone();
            });
        }

        private void hadleCheckList(bool isActive)
        {
            print("check list active");
        }

        private void resetProgress()
        {
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
