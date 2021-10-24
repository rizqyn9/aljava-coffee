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
        [SerializeField] List<MachineIgrendient> igrendientsList = new List<MachineIgrendient>();

        private void OnMouseDown()
        {
            if (MachineState == MachineState.ON_DONE
                && GlassContainer.IsGlassTargetAvaible(MachineIgrendient.NULL, out glassTarget)
                && igrendientsList.Count >= 0
                )
            {
                StartCoroutine(IDestroy());
            }
        }

        public override void OnMachineInit()
        {
            base.OnMachineInit();

        }

        public void reqInput(MachineIgrendient _MachineIgrendient)
        {
            StartCoroutine(ISpawn());
            igrendientsList.Add(_MachineIgrendient);
            igrendientsList.Add(MachineData.MachineType);
        }

        IEnumerator ISpawn()
        {
            MachineState = MachineState.ON_PROCESS;

            yield return 1;

            baseAnimateOnProcess();

            yield break;
        }

        IEnumerator IDestroy()
        {
            glassTarget.glass.changeSpriteAddIgrendients(colorIgrendientsOutput, _multipleIgrendients: igrendientsList);
            glassTarget.glass.process();


            MachineState = MachineState.ON_IDDLE;
            yield break;
        }

        void resetDepends()
        {
            igrendientsList = new List<MachineIgrendient>();
            glassTarget = new GlassRegistered();
        }
    }
}
