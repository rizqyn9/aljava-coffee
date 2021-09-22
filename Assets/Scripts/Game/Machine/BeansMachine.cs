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

        public override void RegistToManager() => CoffeeManager.Instance.beansMachines.Add(this);

        public override void InitStart()
        {
            MachineState = MachineState.ON_IDDLE;
        }

        public void OnMouseDown()
        {
            if (!isGameStarted || MachineState == MachineState.ON_PROCESS) return;

            validate();
        }

        private void validate()
        {
            coffeeMaker = null;
            switch (MachineState)
            {
                case MachineState.ON_IDDLE:
                    StartCoroutine(ISpawn());
                    break;
                case MachineState.ON_DONE:
                    if (isCoffeeMakerIddle() && coffeeMaker)
                    {
                        StartCoroutine(IDestroy());
                    }
                    break;
            }
        }

        IEnumerator ISpawn()
        {
            MachineState = MachineState.ON_PROCESS;

            resultGO = Instantiate(machineType == MachineType.BEANS_ARABICA ? CoffeeManager.Instance.arabicaBeansPrefab : CoffeeManager.Instance.robustaBeansPrefab, resultSpawnPosition);
            resultGO.transform.LeanScale(new Vector2(1f, 1f), .2f);

            yield return new WaitForSeconds(.5f);

            MachineState = MachineState.ON_DONE;
            yield break;
        }

        IEnumerator IDestroy()
        {
            MachineState = MachineState.ON_PROCESS;

            resultGO.transform.LeanScale(new Vector2(0f, 0f), 1);
            yield return new WaitForSeconds(1);
            Destroy(resultGO);

            coffeeMaker.reqInput(resultIgrendients);

            Debug.Log("Send To Coffee Maker");
            MachineState = MachineState.ON_IDDLE;
        }

        bool isCoffeeMakerIddle()
        {
            bool res = MainController.Instance.isMachineAvaible(nextTargetMachine, out Machine _machine);
            coffeeMaker = _machine as CoffeeMaker;

            return res;
        }
    }
}
