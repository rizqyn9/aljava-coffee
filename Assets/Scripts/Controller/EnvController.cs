using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class EnvController : Singleton<EnvController>, IGameState
    {
        [Header("Properties")]
        public Transform mainContainer;
        public Transform flavourContainer;
        public Transform additionalContainer;
        public GlassContainer GlassContainer;
        public GameObject radBarComponent;
        public GameObject capacityBarComponent;

        [Header("Debug")]
        [SerializeField] List<Machine> Machines = new List<Machine>();
        [SerializeField] List<IMenuClassManager> IMenuClassManagers = new List<IMenuClassManager>();
        [SerializeField] GameState gameState;

        #region GAME STATE
        private void OnEnable() => MainController.OnGameStateChanged += GameStateHandler;
        private void OnDisable() => MainController.OnGameStateChanged += GameStateHandler;

        public void GameStateHandler(GameState _gameState)
        {
            GameStateController.UpdateGameState(this, _gameState);
            gameState = _gameState;
        }

        public GameObject GetGameObject() => gameObject;

        public void OnGameIddle()
        {

        }

        public void OnGameBeforeStart() { }

        public void OnGameStart() { }

        public void OnGamePause() { }

        public void OnGameClearance() { }

        public void OnGameFinish() { }

        public void OnGameInit() { }

        #endregion

        private void Start()
        {
            initMachineManager();
            spawnMachine();
        }

        public static void RegistMachine(Machine _machine) => Instance.Machines.Add(_machine);

        /// <summary>
        /// Spawn class menu manager
        /// </summary>
        void initMachineManager()
        {
            //print("InitMachineManager");
            foreach(MenuClassificationData _menuClassification in LevelController.Instance.MenuClassificationDatas)
            {
                GameObject go = Instantiate(_menuClassification.prefabManager, getTransform(_menuClassification.MenuClassification));
                IMenuClassManagers.Add(go.GetComponent<IMenuClassManager>());
            }
            //print($"Count : {IMenuClassManagers.Count}");
        }

        /// <summary>
        /// Notify to spawn manager to instance all depends will be need
        /// </summary>
        private void spawnMachine()
        {
            print("Spawn Machine");
            //print($"Manager Count {IMenuClassManagers.Count}");
            foreach (IMenuClassManager _menuClassManager in IMenuClassManagers)
            {
                //_menuClassManager
                _menuClassManager.InstanceMachine(LevelController.Instance.MachineDatas.FindAll(val=> val.machineClass == _menuClassManager.GetMachineClass()));
            }
        }

        public static void InstanceMachine(MachineData _machineData, Transform _transform, out Machine _machine)
        {
            _machine = Instantiate(_machineData.basePrefab, _transform).GetComponent<Machine>();
            _machine.setMachineData(_machineData, GetMachineLevel(_machineData));
        }

        public static int GetMachineLevel(MachineData _machine)
        {
            int res = MainController.Instance.inLevelUserData.machineLevels.Find(val => val.machineIgrendient == _machine.machineType).level;
            return res == 0 ? 1 : res;
        }

        public static bool FindAndCheckTarget<T>(MachineIgrendient machineType, out T _out) where T : class
        {
            _out = Instance.Machines.Find(val =>
                    val.machineType == machineType
                    && val.MachineState == MachineState.ON_IDDLE
                ) as T;
            return _out != null ? true : false;
        }

        Transform getTransform(MenuClassification _machineClass) =>
            (_machineClass == MenuClassification.COFFEE) ? mainContainer :
            (_machineClass == MenuClassification.ADDITIONAL) ? additionalContainer :
            flavourContainer;

        public static T GetOutType<T>(GameObject _go)
        {
            T _out = _go.GetComponent<T>();
            return _out;
        }
    }
}
