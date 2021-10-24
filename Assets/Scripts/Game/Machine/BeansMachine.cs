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

        public override void OnMachineInit()
        {
            base.OnMachineInit();
            registUIOverlay();
            spawnOverlay = true;
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

            coffeeMaker.reqInput(MachineData.MachineType);

            yield break;
        }
    }
}
