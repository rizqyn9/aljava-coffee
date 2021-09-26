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
                && isGlassTargetAvaible()
                )
            {
                spawnToGlass();
            }
            else
                Debug.Log("gak nemu");
        }

        private void spawnToGlass()
        {
            bool isArabica = glassTarget.glass.igrendients.Contains(enumIgrendients.BEANS_ARABICA);
            SpriteColorCustom _sprite = colorResult.Find(val => val.targetIgrendients == (isArabica ? enumIgrendients.BEANS_ARABICA : enumIgrendients.BEANS_ROBUSTA));
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
            Debug.Log("Destroy");
            Destroy(resultGO);
            spawnResult();
            yield break;
        }

        bool isGlassTargetAvaible()
        {
            glassTarget.glassCode = null;

            glassTarget = GlassContainer.Instance.findGlassLastState(enumIgrendients.COFEE_MAKER);

            if (glassTarget.glassCode == null) return false;
            return true;
        }
    }
}
