using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BeansMachine : Machine
    {
        //[Header("Properties")]

        public override void RegistToManager()
        {
            CoffeeManager.Instance.beansMachines.Add(this);
        }

        public void OnMouseDown()
        {
            if(resultSpawnPosition.childCount == 0)
            {
                spawnResult();
            }
            else if (resultGO && CoffeeManager.Instance.isAcceptabble(machineType == enumMachineType.BEANS_ARABICA ? enumIgrendients.BEANS_ARABICA : enumIgrendients.BEANS_ROBUSTA))
            {
                resultGO.transform.LeanScale(new Vector2(.5f, .5f), .2f);
                Destroy(resultGO);
            }
        }

        private void spawnResult()
        {
            resultGO = Instantiate(machineType == enumMachineType.BEANS_ARABICA ? CoffeeManager.Instance.arabicaBeansPrefab : CoffeeManager.Instance.robustaBeansPrefab, resultSpawnPosition);
            resultGO.transform.LeanScale(new Vector2(1f, 1f), .2f);
        }
    }
}
