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
        public bool firstInit = true;

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
            //if (firstInit)
            //{
            //    firstInitHandler();
            //    yield break;
            //}

            print("Spawn beans");
            MachineState = MachineState.ON_PROCESS;

            resultGO = Instantiate(MachineData.PrefabResult, resultSpawnPosition);
            resultGO.transform.LeanScale(Vector2.zero, 0);
            resultGO.transform.LeanScale(new Vector2(1f, 1f), .8f);

            yield return new WaitForSeconds(.8f);

            MachineState = MachineState.ON_DONE;
            yield break;
        }

        public override void OnMachineProcess()
        {
            baseAnimateOnProcess();
            base.OnMachineProcess();
        }

        private void firstInitHandler()
        {
            print("firstInit machine");
            GameUIController.Instance.reqUseMachineOverlay(MachineData.PrefabUIOverlay);
            firstInit = false;
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
