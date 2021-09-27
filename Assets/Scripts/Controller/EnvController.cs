using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface IEnv
    {
        void EnvInstance();
        void Command();
        public GameState GameState { get; set; }
    }

    public class EnvController : Singleton<EnvController>, IGameState
    {
        [Header("Properties")]
        public Transform mainContainer;
        public Transform flavourContainer;
        public Transform additionalContainer;
        public GlassContainer glassContainer;

        [Header("Debug")]
        [SerializeField] List<IEnv> IEnvs = new List<IEnv>();
        [SerializeField] List<Machine> Machines = new List<Machine>();
        [SerializeField] List<IMenuClassManager> IMenuClassManagers = new List<IMenuClassManager>();
        [SerializeField] GameState _gameState;
        public GameState GameState
        {
            get => _gameState;
            set
            {
                _gameState = value;
            }
        }
        public void OnGameStateChanged() => GameState = MainController.Instance.GameState;

        public void Init()
        {
            MainController.Instance.RegistGameState(this);

            print("Init on Env Manager");
            initMachineManager();
            spawnMachine();
        }

        public void instanceIEnv()
        {
            print($"IENV {IEnvs.Count}");
            foreach(IEnv _env in IEnvs)
            {
                _env.EnvInstance();
            }
        }

        public static void RegistMachine(Machine _machine)
        {
            print("rgist");
            Instance.Machines.Add(_machine);
            if (_machine is IEnv) RegistEnv(_machine);
        }

        [SerializeField] int IEnvRegistered;
        public static void RegistEnv(IEnv _env)
        {
            Instance.IEnvs.Add(_env);
            Instance.IEnvRegistered = Instance.IEnvs.Count;
        }

        /// <summary>
        /// Spawn class menu manager
        /// </summary>
        void initMachineManager()
        {
            print("InitMachineManager");
            foreach(MenuClassificationData _menuClassification in LevelController.Instance.MenuClassificationDatas)
            {
                //GameObject go = Instantiate(_menuClassification.prefabManager, getTransform(_menuClassification.MachineClass.));
                //IMenuClassManagers.Add(go.GetComponent<IMenuClassManager>());
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
            print($"Start Machine {Machines.Count}");
            foreach(Machine _machine in Machines)
            {
                _machine.StartMachine();
            }
        }

        public static void InstanceMachine(MachineData _machineData, Transform _transform, out Machine _machine)
        {
            _machine = Instantiate(_machineData.PrefabManager, _transform).GetComponent<Machine>();
            _machine.MachineData = _machineData;
        }

        public static bool FindAndCheckTarget<T>(MachineType machineType, out T _out) where T : class
        {
            _out = EnvController.Instance.Machines.Find(val =>
                    val.machineType == machineType
                    && val.MachineState == MachineState.ON_IDDLE
                ) as T;
            return _out != null ? true : false;
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
