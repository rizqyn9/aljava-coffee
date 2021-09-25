using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface IEnv
    {
        void EnvInstance();
    }

    public class EnvManager : Singleton<EnvManager>
    {
        [Header("Properties")]
        public Transform mainContainer;
        public Transform flavourContainer;
        public Transform additionalContainer;
        public GlassContainer glassContainer;

        [Header("Debug")]
        LevelBase levelBase = null;
        [SerializeField] List<GameObject> resGO = new List<GameObject>();
        [SerializeField] List<IEnv> IEnvs = new List<IEnv>();
        [SerializeField] List<IMenuClassManager> IMenuClassManagers = new List<IMenuClassManager>();
        [SerializeField] List<Machine> Machines = new List<Machine>();

        public void Init()
        {
            print("Init on Env Manager");
            initMachineManager();
            spawnMachine();
        }

        public void RegistMachine(Machine _machine) => Machines.Add(_machine);

        /// <summary>
        /// Spawn class menu manager
        /// </summary>
        void initMachineManager()
        {
            print("InitMachineManager");
            foreach(MenuClassificationData _menuClassification in LevelManager.Instance.MenuClassificationDatas)
            {
                GameObject go = Instantiate(_menuClassification.prefabManager, getTransform(_menuClassification.MachineClass));
                IMenuClassManagers.Add(go.GetComponent<IMenuClassManager>());
            }
            print($"Count : {IMenuClassManagers.Count}");
        }

        /// <summary>
        /// Notify to spawn manager to instance all depends will be need
        /// </summary>
        private void spawnMachine()
        {
            print("Spawn Machine");
            foreach(IMenuClassManager _menuClassManager in IMenuClassManagers)
            {
                _menuClassManager.InstanceMachine(LevelManager.Instance.MachineDatas.FindAll(val=> val.MachineClass == _menuClassManager.GetMachineClass()));
            }
        }

        Transform getTransform(MachineClass _machineClass) =>
            (_machineClass == MachineClass.COFFEE) ? mainContainer :
            (_machineClass == MachineClass.ADDITIONAL) ? additionalContainer :
            flavourContainer;

        public static T GetOutType<T>(GameObject _go)
        {
            T _out = _go.GetComponent<T>();
            return _out;
        }
    }
}
