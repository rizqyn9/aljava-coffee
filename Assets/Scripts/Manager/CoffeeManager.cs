using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CoffeeManager : Singleton<CoffeeManager>
    {
        [Header("Properties")]
        public GameObject arabicaBeansPrefab;
        public GameObject robustaBeansPrefab;

        [Header("Debug")]
        public List<CoffeeMaker> coffeeMakers = new List<CoffeeMaker>();
        public List<BeansMachine> beansMachines = new List<BeansMachine>();
        public MilkSteam milkSteam;

        public bool isAcceptabble(enumIgrendients _enumIgrendients)
        {
            return findCoffeeMaker(_enumIgrendients);
        }
        public enumIgrendients test;
        public bool findCoffeeMaker(enumIgrendients _enumIgrendients)
        {
            CoffeeMaker coffeeMaker = coffeeMakers.Find(res => res.enumMachineState == enumMachineState.ON_IDDLE);
            if (!coffeeMaker) return false;
            test = _enumIgrendients;
            coffeeMaker.spawnRes(_enumIgrendients);
            return true;
        }
    }
}
