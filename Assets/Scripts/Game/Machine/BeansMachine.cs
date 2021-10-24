using System.Collections;
using UnityEngine;

namespace Game
{
    public class BeansMachine : Machine
    {
        [Header("Debug")]
        [SerializeField] CoffeeMaker coffeeMaker;
        [SerializeField] bool firstInit = true;

        public void OnMouseDown()
        {
            if (!isInteractable()) return;
            if (MachineState == MachineState.ON_IDDLE) StartCoroutine(ISpawn());
            if (MachineState == MachineState.ON_DONE) validate();
            
        }

        private void validate()
        {
            if (CapacityMachine.stateCapacity == 0) return;
            if (EnvController.FindAndCheckTarget(MachineData.TargetMachine, out coffeeMaker))
            {
                CapacityMachine.getOne();
                StartCoroutine(IThrowResult());
            }
        }

        private void firstInitHandler()
        {
            GameUIController.Instance.reqUseMachineOverlay(this);
            firstInit = false;
        }


        IEnumerator ISpawn()
        {
            MachineState = MachineState.ON_PROCESS;

            yield break;
        }

        IEnumerator IThrowResult()
        {

            coffeeMaker.ReqInput(MachineData.MachineType);

            yield break;
        }
    }
}
