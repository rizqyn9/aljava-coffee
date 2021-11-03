using System.Collections;
using UnityEngine;

namespace Game
{
    public class BeansMachine : Machine
    {
        [Header("Debug")]
        [SerializeField] CoffeeMaker coffeeMaker;

        public void OnMouseDown()
        {
            if (!isInteractable()) return;
            if (spawnOverlay)
            {
                handleSpawnOverlay();
                return;
            }

            if (MachineState == MachineState.ON_IDDLE) StartCoroutine(ISpawn());
            if (MachineState == MachineState.ON_DONE) validate();
        }

        private void validate()
        {
            if (capacityMachine.stateCapacity == 0) return;
            if (EnvController.FindAndCheckTarget(machineData.targetMachine, out coffeeMaker))
            {
                capacityMachine.getOne();
                OnMachineServe();
                StartCoroutine(IThrowResult());
            }
        }

        void handleSpawnOverlay()
        {
            machineUI.reqInstance();
        }

        IEnumerator ISpawn()
        {
            MachineState = MachineState.ON_PROCESS;

            yield break;
        }

        IEnumerator IThrowResult()
        {
            coffeeMaker.reqInput(machineData.machineType);

            yield break;
        }
    }
}
