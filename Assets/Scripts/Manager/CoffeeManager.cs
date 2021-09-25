using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface ICoffeeManager
    {
        void Init();
    }

    public class CoffeeManager : Singleton<CoffeeManager>,IEnv, IMenuClassManager
    {
        [Header("Properties")]
        public MachineClass MachineClass = MachineClass.COFFEE;
        public GameObject arabicaBeansPrefab;
        public GameObject robustaBeansPrefab;
        public List<MachineData> MachineDatas;

        [Header("Debug")]
        public List<ICoffeeManager> coffeeManagers = new List<ICoffeeManager>();
        public List<Machine> machineInstance = new List<Machine>();

        public void InstanceMachine(List<MachineData> _machineDatas)
        {
            MachineDatas = _machineDatas;
            renderMachines();
        }

        private void renderMachines()
        {
            foreach(MachineData _machine in MachineDatas)
            {
                GameObject go = Instantiate(_machine.PrefabManager, transform);
                coffeeManagers.Add(go.GetComponent<ICoffeeManager>());
                machineInstance.Add(go.GetComponent<Machine>());
            }
        }

        public MachineClass GetMachineClass() => MachineClass;

        private void Start()
        {
            name = "--CoffeeManager";
        }

        public void EnvInstance()
        {
            throw new System.NotImplementedException();
        }
    }
}
