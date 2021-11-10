using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface IAdditionalManager
    {

    }

    public class AdditionalManager : Singleton<AdditionalManager>, IMenuClassManager
    {
        [Header("Properties")]
        public MachineClass MachineClass = MachineClass.ADDITIONAL;

        [Header("Debug")]
        [SerializeField] MenuClassificationData MenuClassificationData;
        [SerializeField] List<MachineData> MachineDatas;
        [SerializeField] List<IAdditionalManager> additionalManagers;
        [SerializeField] List<Machine> machineInstance = new List<Machine>();

        public MachineClass GetMachineClass() => MachineClass;

        public void InstanceMachine(List<MachineData> _machineDatas)
        {
            //print("Additional Manager");
            MachineDatas = _machineDatas;
            renderMachines();
        }

        private void renderMachines()
        {
            //print("Render Machine in Coffee Manager");
            foreach (MachineData _machine in MachineDatas)
            {
                Machine resMachine;
                EnvController.InstanceMachine(_machine, transform, out resMachine);
                if(resMachine is IAdditionalManager) additionalManagers.Add(resMachine as IAdditionalManager);
                machineInstance.Add(resMachine);
            }
        }
    }
}
