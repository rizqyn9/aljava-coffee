using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;

namespace Game
{
    public interface IEnv
    {
        void EnvInstance();
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
        [SerializeField] ObservableCollection<IEnv> IEnvs = new ObservableCollection<IEnv>();
        [SerializeField] List<IMenuClassManager> IMenuClassManagers = new List<IMenuClassManager>();
        [SerializeField] List<Machine> Machines = new List<Machine>();
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

            IEnvs.CollectionChanged += updateListEnv;
            print("Init on Env Manager");
            initMachineManager();
            spawnMachine();
        }

        [SerializeField] int IEnvRegistered;
        private void updateListEnv(object sender, NotifyCollectionChangedEventArgs e) => IEnvRegistered = IEnvs.Count;

        public void RegistMachine(Machine _machine)
        {
            Machines.Add(_machine);
            if(_machine is IEnv) IEnvs.Add(_machine);   // Register machine as ienv is avaible
        }

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
