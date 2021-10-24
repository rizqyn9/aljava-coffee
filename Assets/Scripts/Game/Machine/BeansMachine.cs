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
            if (EnvController.FindAndCheckTarget(MachineData.TargetMachine, out coffeeMaker))
            {
                if(CapacityMachine.stateCapacity == 0)
                {
                    StartCoroutine(IDestroy());
                }
                CapacityMachine.getOne();
            }
        }

        private void firstInitHandler()
        {
            GameUIController.Instance.reqUseMachineOverlay(this);
            firstInit = false;
        }

        IEnumerator ISpawn()
        {
            if (firstInit)
            {
                firstInitHandler();
                yield break;
            }

            MachineState = MachineState.ON_PROCESS;

            CapacityMachine.setFull();

            resultGO = Instantiate(MachineData.PrefabResult, resultSpawnPosition);
            resultGO.transform.LeanScale(Vector2.zero, 0);
            resultGO.transform.LeanScale(new Vector2(1f, 1f), .8f);

            yield return new WaitForSeconds(.8f);

            MachineState = MachineState.ON_DONE;
            yield break;
        }

        IEnumerator IDestroy()
        {
            MachineState = MachineState.ON_CLEARANCE;

            resultGO.transform.LeanScale(new Vector2(0f, 0f), .2f);
            yield return new WaitForSeconds(.2f);
            Destroy(resultGO);

            coffeeMaker.ReqInput(MachineData.MachineType);

            Debug.Log("Send To Coffee Maker");
            MachineState = MachineState.ON_IDDLE;
        }
    }
}
