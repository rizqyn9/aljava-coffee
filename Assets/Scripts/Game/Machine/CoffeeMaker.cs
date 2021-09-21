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
        public Color colorIgrendientsOutput;
        public enumIgrendients resultIgrendients = enumIgrendients.COFEE_MAKER;

        [Header("Debug")]
        [SerializeField] GlassRegistered glassTarget;
        [SerializeField] List<enumIgrendients> igrendientsList;

        public override void RegistToManager()
        {
            CoffeeManager.Instance.coffeeMakers.Add(this);
        }

        public void spawnRes(enumIgrendients _enumIgrendients)
        {
            igrendientsList.Add(_enumIgrendients);
            enumMachineState = enumMachineState.ON_PROCESS;
            resultGO = Instantiate(resultPrefab, resultSpawnPosition);
            enumMachineState = enumMachineState.ON_DONE;
        }

        private void OnMouseDown()
        {
            if(resultSpawnPosition.childCount != 0)
            {
                glassTarget = GlassContainer.Instance.findEmptyGlass();
                //glassTarget.glass.addMultipleIgrendients(enumIgrendients == enumIgrendients.BEANS_ARABICA ? prefabArabica : prefabRobusta, new List<enumIgrendients> { enumIgrendients, resultIgrendients} ,enumIgrendients);
                //use new method
                igrendientsList.Add(resultIgrendients);
                glassTarget.glass.changeSpriteAddIgrendients(colorIgrendientsOutput, igrendientsList);
                Destroy(resultGO);
                resetState();
            }
        }

        private void resetState()
        {
            enumMachineState = enumMachineState.ON_IDDLE;
            igrendientsList = new List<enumIgrendients>();
            
        }
    }
}
