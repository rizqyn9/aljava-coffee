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
        [SerializeField] List<enumIgrendients> igrendientsList;

        public override void RegistToManager() {}

        public override void InitStart()
        {
            MachineState = MachineState.ON_IDDLE;
            igrendientsList = new List<enumIgrendients>();
        }

        public void reqInput(enumIgrendients _enumIgrendients)
        {
            StartCoroutine(ISpawn());
            igrendientsList.Add(_enumIgrendients);
            igrendientsList.Add(resultIgrendients);
        }

        IEnumerator ISpawn()
        {
            MachineState = MachineState.ON_PROCESS;

            resultGO = Instantiate(resultPrefab, resultSpawnPosition);
            resultGO.LeanScale(new Vector2(1f, 1f), .8f);
            yield return new WaitForSeconds(.8f);

            MachineState = MachineState.ON_DONE;
            yield break;
        }

        private void OnMouseDown()
        {
            if (resultSpawnPosition.childCount > 0
                && isGlassAvaible()
                && MachineState != MachineState.ON_PROCESS
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

            InitStart();
            yield break;
        }

        bool isGlassAvaible()
        {
            glassTarget.glassCode = null;

            glassTarget = GlassContainer.Instance.findGlassLastState(enumIgrendients.NULL);

            if (glassTarget.glassCode == null) return false;
            return true;
        }
    }
}
