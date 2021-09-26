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

    public class EnvController : Singleton<EnvController>, IController
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
        [SerializeField] GameState _gameState;

        public GameState GameState { get; set; }

        public void Init()
        {
            MainController.Instance.AddController(this);
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
            foreach(MenuClassificationData _menuClassification in LevelController.Instance.MenuClassificationDatas)
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
                _menuClassManager.InstanceMachine(LevelController.Instance.MachineDatas.FindAll(val=> val.MachineClass == _menuClassManager.GetMachineClass()));
            }
        }

        /// <summary>
        /// Turn on all machine
        /// </summary>
        internal void StartMachine()
        {
            foreach(Machine _machine in Machines)
            {
                _machine.InitStart();
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
