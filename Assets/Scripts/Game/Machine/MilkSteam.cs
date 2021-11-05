using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MilkSteam : Machine
    {
        [Header("Debug")]
        public GlassRegistered glassTarget;

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
            Sprite _sprite = getSprite(glassTarget.glass.igrendients[0]);
            glassTarget.glass.changeSpriteAddIgrendients(_sprite);
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
            barMachine.resetProgress();

            spawnResult();

            yield break;
        }
    }
}
