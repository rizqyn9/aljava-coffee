using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CoffeeManager : Singleton<CoffeeManager>
    {
        public List<CoffeeMaker> coffeeMakers = new List<CoffeeMaker>();
        public List<BeansMachine> beansMachines = new List<BeansMachine>();
        public GameObject arabicaBeansPrefab;
        public GameObject robustaBeansPrefab;

        public bool isAcceptabble(enumIgrendients _enumIgrendients)
        {
            return findCoffeeMaker(_enumIgrendients);
        }

        public bool findCoffeeMaker(enumIgrendients _enumIgrendients)
        {
            CoffeeMaker coffeeMaker = coffeeMakers.Find(res => res.enumMachineState == enumMachineState.ON_IDDLE);
            if (!coffeeMaker) return false;

            coffeeMaker.spawnRes(_enumIgrendients);
            return true;
        }
    }
}
