using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MilkSteam : Machine
    {
        [Header("Properties")]
        public List<SpriteColorCustom> colorResult;
        public float delay;

        [Header("Debug")]
        public GlassRegistered glassTarget;

        private void Start() => delay = MachineData.durationProcess;

        public override void OnMachineIddle()
        {
            spawnResult();
        }

        private void OnMouseDown()
        {
            if (MachineState == MachineState.ON_DONE
                && GlassContainer.IsGlassTargetAvaible(MachineIgrendient.COFEE_MAKER, out glassTarget)
                )
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
            glassTarget.glass.changeSpriteAddIgrendients(_sprite.color, MachineData.MachineType);
            glassTarget.glass.process();

            StartCoroutine(IDestroy());
        }

        private void spawnResult() => StartCoroutine(ISpawn());

        IEnumerator ISpawn()
        {
            MachineState = MachineState.ON_PROCESS;

            resultGO = Instantiate(resultPrefab, resultSpawnPosition);
            resultGO.transform.localScale = Vector2.zero;
            resultGO.LeanScale(new Vector2(1, 1), delay);

            yield return new WaitForSeconds(delay);

            MachineState = MachineState.ON_DONE;
            yield break;
        }

        IEnumerator IDestroy()
        {
            MachineState = MachineState.ON_PROCESS;

            Debug.Log("Destroy");
            Destroy(resultGO);
            spawnResult();

            MachineState = MachineState.ON_IDDLE;
            yield break;
        }
    }
}
