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
                OnMachineSpawnOverlay();
                return;
            }

            if (MachineState == MachineState.ON_IDDLE) OnMachineSpawn();
            if (MachineState == MachineState.ON_DONE) OnValidate();
        }

        public override void validateLogic()
        {
            if (capacityMachine.stateCapacity == 0) return;
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
