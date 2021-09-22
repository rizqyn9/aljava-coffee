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
    }
}
