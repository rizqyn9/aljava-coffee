using UnityEngine;
using Game;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Machine : MonoBehaviour, IEnv, IGameState
{
    [Header("Propertie Machine")]
    public GameObject resultPrefab;
    public Transform resultSpawnPosition;
    public MachineType machineType;
    public MachineType nextTargetMachine;
    public enumIgrendients resultIgrendients;
    public bool spawnOnStart;

    [Header("Debug")]
    public MachineData MachineData;

    [SerializeField] MachineState _machineState;
    [SerializeField] GameState _gameState;
    [SerializeField] protected GameObject resultGO;
    [SerializeField] protected BoxCollider2D boxCollider2D;

    public MachineState MachineState
    {
        get => _machineState;
        set
        {
            if (_machineState == value) return;
            OnMachineStateChanged(_machineState, value);
            _machineState = value;
        }
    }

    public GameState GameState
    {
        get => _gameState;
        set
        {
            if (_gameState == value) return;
            OnGameStateChanged(_gameState, value);
            _gameState = value;
        }
    }

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        // regist to env manager
        MainController.Instance.RegistGameState(this);

        MachineState = MachineState.OFF;

        EnvController.Instance.RegistMachine(this);
    }

    public void StartMachine()
    {
        MachineState = MachineState.ON_IDDLE;
        InitStart();
    }

    public abstract void InitStart();

    public virtual void EnvInstance() { }

    public virtual void OnGameStateChanged(GameState _old, GameState _new) { }

    public virtual void OnMachineStateChanged(MachineState _old, MachineState _new) { }

    public void OnGameStateChanged()
    {
        boxCollider2D.enabled = GameState != GameState.IDDLE;
    }
}
