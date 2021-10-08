using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CoffeeMaker : Machine
    {
        [Header("Properties")]
        public Color colorIgrendientsOutput;

        [Header("Debug")]
        [SerializeField] GlassRegistered glassTarget;
        [SerializeField] List<MachineIgrendient> igrendientsList;

        public override void OnMachineIddle()
        {
            base.OnMachineIddle();
            igrendientsList = new List<MachineIgrendient>();
        }

        public void ReqInput(MachineIgrendient _MachineIgrendient)
        {
            StartCoroutine(ISpawn());
            igrendientsList.Add(_MachineIgrendient);
            igrendientsList.Add(MachineData.MachineType);
        }

        IEnumerator ISpawn()
        {
            MachineState = MachineState.ON_PROCESS;

            baseAnimateOnProcess();

            resultGO = Instantiate(resultPrefab, resultSpawnPosition);
            resultGO.LeanScale(new Vector2(1f, 1f), .8f);
            yield return new WaitForSeconds(.8f);

            MachineState = MachineState.ON_DONE;
            yield break;
        }

        private void OnMouseDown()
        {
            if (MachineState == MachineState.ON_DONE
                && GlassContainer.IsGlassTargetAvaible(MachineIgrendient.NULL, out glassTarget)
                )
            {
                StartCoroutine(IDestroy());
            }
        }

        IEnumerator IDestroy()
        {
            MachineState = MachineState.ON_PROCESS;

            glassTarget.glass.changeSpriteAddIgrendients(colorIgrendientsOutput, _multipleIgrendients: igrendientsList);
            glassTarget.glass.process();

            resultGO.LeanScale(new Vector2(0, 0), .2f);
            yield return new WaitForSeconds(.2f);

            Destroy(resultGO);

            MachineState = MachineState.ON_IDDLE;
            yield break;
        }
    }
}
