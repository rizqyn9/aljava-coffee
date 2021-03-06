using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface ICoffeeManager
    {
        void Init();
    }

    public class CoffeeManager : Singleton<CoffeeManager>, IMenuClassManager
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
            print("Coffee Manager");
            MachineDatas = _machineDatas;
            renderMachines();
        }

        private void renderMachines()
        {
            print("Render Machine in Coffee Manager");
            foreach(MachineData _machine in MachineDatas)
            {
                Machine resMachine;
                EnvController.InstanceMachine(_machine, transform, out resMachine);
                coffeeManagers.Add(resMachine as ICoffeeManager);
                machineInstance.Add(resMachine);
            }
        }

        public MachineClass GetMachineClass() => MachineClass;

        private void Start()
        {
            name = "--CoffeeManager";
        }
    }
}
