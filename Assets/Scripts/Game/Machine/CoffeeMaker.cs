using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CoffeeMaker : Machine
    {

        [Header("Properties")]
        public GameObject prefabArabica;
        public GameObject prefabRobusta;
        public enumIgrendients resultIgrendients = enumIgrendients.COFEE_MAKER;

        [Header("Debug")]
        [SerializeField] GlassRegistered glassTarget;
        [SerializeField] enumIgrendients enumIgrendients;

        public override void RegistToManager()
        {
            CoffeeManager.Instance.coffeeMakers.Add(this);
        }

        public void spawnRes(enumIgrendients _enumIgrendients)
        {
            enumIgrendients = _enumIgrendients;
            enumMachineState = enumMachineState.ON_PROCESS;
            resultGO = Instantiate(resultPrefab, resultSpawnPosition);
            enumMachineState = enumMachineState.ON_DONE;
        }

        private void OnMouseDown()
        {
            if(resultSpawnPosition.childCount != 0)
            {
                glassTarget = GlassContainer.Instance.findEmptyGlass();
                glassTarget.glass.addMultipleIgrendients(enumIgrendients == enumIgrendients.BEANS_ARABICA ? prefabArabica : prefabRobusta, new List<enumIgrendients> { enumIgrendients, resultIgrendients} ,enumIgrendients);
                Destroy(resultGO);
                resetState();
            }
        }

        private void resetState()
        {
            enumMachineState = enumMachineState.ON_IDDLE;
            
        }
    }
}
