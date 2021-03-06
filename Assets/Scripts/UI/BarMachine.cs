using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Game
{
    /// <summary>
    /// Radius Bar
    /// </summary>
    public class BarMachine : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] Image bar;
        [SerializeField] GameObject checkListGO;
        [SerializeField] GameObject repairListGO;

        [Tooltip("Index0: default color\nIndex1: overcook color")]
        [SerializeField] Color32[] colors;

        [Header("Debug")]
        [SerializeField] int leanTweenID;
        [SerializeField] float time;
        [SerializeField] Machine machine;
        [SerializeField] bool isActive;
        public enum BarType
        {
            OVERCOOK,
            DEFAULT,
            NOT_SET
        }

        private void Start()
        {
            bar.fillAmount = 0; // Ensure fill equals to zero
        }

        public void init(Machine _machine)
        {
            machine = _machine;
            gameObject.name = $"{_machine.gameObject.name}--radius-bar";
            transform.position = Camera.main.WorldToScreenPoint(_machine.radiusBarPos.position);

            time = machine.machineProperties.processDuration;
        }

        public void runProgress(BarType _barType)
        {
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

            bar.color = (_barType == BarType.OVERCOOK) ? colors[1] : colors[0];

            leanTweenID = LeanTween.value(0, 100, (_barType == BarType.OVERCOOK) ? GlobalController.Instance.overCookDuration : time).setOnUpdate((float val) =>
            {
                bar.fillAmount = val / 100;
            }).setOnComplete(()=>
            {
                machine.setColliderEnabled();       /// Enabled machine collider
                if (machine.isUseBarCapacity)
                {
                    resetProgress();
                } else
                {
                    if (machine.isUseOverCook)
                    {
                        if(_barType == BarType.OVERCOOK)
                        {
                            machine.initRepair();
                            uiHandleState(_barType);
                        } else
                        {
                            StartCoroutine(machine.I_InitOverCook());
                        }
                        return;
                    }
                    return;
                }
                machine.barMachineDone();
                isActive = false;
            }).id;

            yield break;
        }

        public void stopBar()
        {
            LeanTween.cancel(leanTweenID);
            resetProgress();
        }

        public void uiHandleState(BarType _barType)
        {
            repairListGO.SetActive(_barType == BarType.OVERCOOK);
            checkListGO.SetActive(_barType == BarType.DEFAULT);
        }

        public void resetProgress()
        {
            isActive = false;

            bar.fillAmount = 0;
            uiHandleState(BarType.NOT_SET);

            hideBar();
        }

        void hideBar()
        {
            gameObject.LeanAlpha(0, .3f);
            transform.LeanScale(Vector2.zero, .3f);
        }

        private void OnEnable()
        {
            gameObject.LeanAlpha(0, 0);
            transform.LeanScale(Vector2.zero, 0);
        }

        internal void simulateRepair()
        {
            LeanTween.rotateZ(repairListGO, 30f, GlobalController.Instance.repairMachineDuration / 5).setLoopPingPong(5).setOnComplete(() =>
            {
                machine.OnMachineSuccessRepair();
            });
        }
    }
}
