using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MilkSteam : Machine
    {
        [Header("Properties")]
        public List<SpriteColorCustom> colorResult;

        [Header("Debug")]
        public GlassRegistered glassTarget;

        public override void OnMachineInit()
        {
            useRadiusBar();
        }

        public override void OnMachineIddle() => spawnResult();

        private void OnMouseDown()
        {
            if (MachineState != MachineState.ON_DONE) return;
            if (GlassContainer.IsGlassTargetAvaible(MachineIgrendient.COFEE_MAKER, out glassTarget))
            {
                spawnToGlass();
            }
            else
                Debug.LogWarning("gak nemu");
        }

        //FIXME
        /// <summary>
        /// Need fixed for dynamically value
        /// </summary>
        private void spawnToGlass()
        {
            bool isArabica = glassTarget.glass.igrendients.Contains(MachineIgrendient.BEANS_ARABICA);
            SpriteColorCustom _sprite = colorResult.Find(val => val.targetIgrendients == (isArabica ? MachineIgrendient.BEANS_ARABICA : MachineIgrendient.BEANS_ROBUSTA));
            glassTarget.glass.changeSpriteAddIgrendients(_sprite.color, machineData.machineType);
            glassTarget.glass.process();

            StartCoroutine(IDestroy());
        }

        private void spawnResult() => StartCoroutine(ISpawn());

        IEnumerator ISpawn()
        {
            MachineState = MachineState.ON_PROCESS;
            //Debug.LogWarning("Machine process");

            yield break;
        }

        IEnumerator IDestroy()
        {
            MachineState = MachineState.ON_IDDLE;
            BarMachine.resetProgress();

            spawnResult();

            yield break;
        }
    }
}
