using UnityEngine;
using Game;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Machine : MonoBehaviour
{
    [Header("Propertie Machine")]
    public GameObject resultPrefab;
    public Transform resultSpawnPosition;
    public MachineType machineType;
    public MachineType nextTargetMachine;
    public enumIgrendients resultIgrendients;
    public bool spawnOnStart;

    [Header("Debug")]
    public MachineState MachineState;
    [SerializeField] GameState _gameState;
    [SerializeField] protected GameObject resultGO;
    [SerializeField] protected BoxCollider2D boxCollider2D;

    public GameState GameState
    {
        get => _gameState;
        set => _gameState = value;
    }

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        // regist to env manager
        EnvManager.Instance.RegistMachine(this);
        RegistToManager();
    }

    public void StartMachine() => InitStart();

    public abstract void RegistToManager();

    public abstract void InitStart();
}
