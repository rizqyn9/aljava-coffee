using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BeansMachine : Machine
    {
        [Header("Debug")]
        public CoffeeMaker coffeeMaker;

        public override void InitStart()
        {
            MachineState = MachineState.ON_IDDLE;
        }

        public override void OnMachineStateChanged(MachineState _old, MachineState _new)
        {
            print("Machine State Changed");
        }

        public void OnMouseDown()
        {
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
            print("Spawn beans");
            MachineState = MachineState.ON_PROCESS;

            resultGO = Instantiate(MachineData.PrefabResult, resultSpawnPosition);
            resultGO.transform.LeanScale(new Vector2(1f, 1f), .8f);

            yield return new WaitForSeconds(.8f);

            MachineState = MachineState.ON_DONE;
            yield break;
        }

        IEnumerator IDestroy()
        {
            MachineState = MachineState.ON_PROCESS;

            resultGO.transform.LeanScale(new Vector2(0f, 0f), .2f);
            yield return new WaitForSeconds(.2f);
            Destroy(resultGO);

            coffeeMaker.ReqInput(MachineData.MachineType);

            Debug.Log("Send To Coffee Maker");
            MachineState = MachineState.ON_IDDLE;
        }
    }
}
