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
                glassTarget = GlassContainer.Instance.findGlassWithState(new List<enumIgrendients> { enumIgrendients.BEANS_ARABICA, enumIgrendients.BEANS_ROBUSTA });
                if (glassTarget.glassCode == null)
                {
                    Debug.Log("Gk nemu");
                    return;
                }
                glassTarget.glass.changeSpriteIgrendients(sprite, resultIgrendients);
                Destroy(resultGO);
                spawnResult();
            }
        }
    }
}
