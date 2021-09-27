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

        public override void InitStart()
        {
            MachineState = MachineState.ON_IDDLE;
            spawnResult();
        }

        private void spawnResult()
        {
            StartCoroutine(ISpawn());
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
                Debug.Log("gak nemu");
        }

        private void spawnToGlass()
        {
            bool isArabica = glassTarget.glass.igrendients.Contains(MachineIgrendient.BEANS_ARABICA);
            SpriteColorCustom _sprite = colorResult.Find(val => val.targetIgrendients == (isArabica ? MachineIgrendient.BEANS_ARABICA : MachineIgrendient.BEANS_ROBUSTA));
            glassTarget.glass.changeSpriteAddIgrendients(_sprite.color, resultIgrendients);
            glassTarget.glass.process();

            StartCoroutine(IDestroy());
        }

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
