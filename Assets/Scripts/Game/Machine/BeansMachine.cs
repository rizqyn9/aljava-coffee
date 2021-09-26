using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BeansMachine : Machine, IEnv
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
            if (MachineState == MachineState.ON_DONE) StartCoroutine(IDestroy());

        }

        //private void validate()
        //{
        //    coffeeMaker = null;
        //    switch (MachineState)
        //    {
        //        case MachineState.ON_IDDLE:
        //            StartCoroutine(ISpawn());
        //            break;
        //        case MachineState.ON_DONE:
        //            if (isCoffeeMakerIddle() && coffeeMaker)
        //            {
        //                StartCoroutine(IDestroy());
        //            }
        //            break;
        //    }
        //}

        IEnumerator ISpawn()
        {
            print("Spawn beans");
            MachineState = MachineState.ON_PROCESS;

            resultGO = Instantiate(MachineData.PrefabResult, resultSpawnPosition);
            resultGO.transform.LeanScale(new Vector2(1f, 1f), .2f);

            yield return new WaitForSeconds(.2f);

            MachineState = MachineState.ON_DONE;
            yield break;
        }

        IEnumerator IDestroy()
        {
            MachineState = MachineState.ON_PROCESS;

            resultGO.transform.LeanScale(new Vector2(0f, 0f), .2f);
            yield return new WaitForSeconds(.2f);
            Destroy(resultGO);

            coffeeMaker.reqInput(resultIgrendients);

            Debug.Log("Send To Coffee Maker");
            MachineState = MachineState.ON_IDDLE;
        }

        //bool isCoffeeMakerIddle()
        //{
        //    bool res = MainController.Instance.isMachineAvaible(nextTargetMachine, out Machine _machine);
        //    coffeeMaker = _machine as CoffeeMaker;

        //    return res;
        //}

        public override void EnvInstance()
        {
            print("spawn");
            gameObject.LeanMoveLocalY(-1f, 1f);
            gameObject.LeanAlpha(1, 1f);
        }
    }
}
