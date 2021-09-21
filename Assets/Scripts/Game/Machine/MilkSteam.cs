using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MilkSteam : Machine
    {
        [Header("Properties")]
        public Sprite sprite;
        public enumIgrendients resultIgrendients = enumIgrendients.MILK_STEAMMED;
        public List<Color> colorResult;

        [Header("Debug")]
        public GlassRegistered glassTarget;

        public override void RegistToManager()
        {
            CoffeeManager.Instance.milkSteam = this;
            spawnResult();
        }

        private void spawnResult()
        {
            StartCoroutine(SpawnRoutine());
        }

        IEnumerator SpawnRoutine()
        {
            yield return new WaitForSeconds(4f);
            resultGO = Instantiate(resultPrefab, resultSpawnPosition);

        }

        private void OnMouseDown()
        {
            if(resultSpawnPosition.childCount == 0)
            {
                spawnResult();
            } else
            {
                glassTarget = GlassContainer.Instance.findGlassWithState(new List<enumIgrendients> { enumIgrendients.COFEE_MAKER });
                if (glassTarget.glassCode == null)
                {
                    Debug.Log("Gk nemu");
                    return;
                }
                bool isArabica = glassTarget.glass.igrendients.Contains(enumIgrendients.BEANS_ARABICA);
                glassTarget.glass.changeSpriteAddIgrendients(isArabica ? colorResult[1] : colorResult[0], new List<enumIgrendients> { resultIgrendients });
                Destroy(resultGO);
                spawnResult();
            }
        }
    }
}
