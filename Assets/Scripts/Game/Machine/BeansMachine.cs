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
            print("click");
            if (!isInteractable()) return;
            if (spawnOverlay)
            {
                OnMachineSpawnOverlay();
                return;
            }

            if (MachineState == MachineState.ON_IDDLE) OnMachineSpawn();
            if (MachineState == MachineState.ON_DONE) OnMachineValidate();
        }

        public override void validateLogic()
        {
            if (capacityMachine.stateCapacity == 0)
            {
                MachineState = MachineState.ON_IDDLE;
                return;
            }
            if (EnvController.FindAndCheckTarget(machineData.targetMachine, out coffeeMaker))
            {
                OnMachineServe();
                //StartCoroutine(IThrowResult());
                coffeeMaker.reqInput(machineData.machineType);
            }
        }

        // Can depreceated
        IEnumerator IThrowResult()
        {
            yield break;
        }
    }
}
