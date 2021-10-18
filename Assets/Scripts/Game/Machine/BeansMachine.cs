using System.Collections;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// TODO
    /// On first init trigger UI overlay
    /// Add Prefab overlay
    /// FIXME
    /// Machine State
    /// </summary>
    public class BeansMachine : Machine
    {
        [Header("Debug")]
        [SerializeField] CoffeeMaker coffeeMaker;
        [SerializeField] bool firstInit = true;

        public override void OnMachineStateChanged(MachineState _old, MachineState _new)
        {
            base.OnMachineStateChanged(_old, _new);
        }

        public void OnMouseDown()
        {
            if (gameState != GameState.START) return;
            if (MachineState == MachineState.ON_IDDLE) StartCoroutine(ISpawn());
            if (MachineState == MachineState.ON_DONE) validate();
        }

        private void validate()
        {
            if (EnvController.FindAndCheckTarget(MachineData.TargetMachine, out coffeeMaker))
            {
                StartCoroutine(IDestroy());
            }
        }

        IEnumerator ISpawn()
        {
            if (firstInit)
            {
                firstInitHandler();
                yield break;
            }

            print("Spawn beans");
            MachineState = MachineState.ON_PROCESS;

            resultGO = Instantiate(MachineData.PrefabResult, resultSpawnPosition);
            resultGO.transform.LeanScale(Vector2.zero, 0);
            resultGO.transform.LeanScale(new Vector2(1f, 1f), .8f);

            yield return new WaitForSeconds(.8f);

            MachineState = MachineState.ON_DONE;
            yield break;
        }

        //public override void OnMachineProcess()
        //{
        //    base.OnMachineProcess();

        //    baseAnimateOnProcess();
        //    BarMachine.StartBar();
        //}

        public override void OnMachineDone()
        {
            base.OnMachineDone();
        }

        public override void OnMachineClearance()
        {
            base.OnMachineClearance();
        }

        private void firstInitHandler()
        {
            //print("firstInit machine");
            GameUIController.Instance.reqUseMachineOverlay(MachineData);
            firstInit = false;
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
