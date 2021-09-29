using UnityEngine;
using Game;
using System;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Machine : MonoBehaviour, IEnv, IGameState
{
    [Header("Propertie Machine")]
    public GameObject resultPrefab;
    public Transform resultSpawnPosition;
    public MachineIgrendient machineType;

    [Header("Debug")]
    public MachineData MachineData;
    [SerializeField] Vector2 basePos;
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

    public void SetMachineData(MachineData _machineData)
    {
        MachineData = _machineData;
        machineType = _machineData.MachineType;
    }

    private void Start()
    {
        //MachineData = LevelController.Instance.MachineDatas.Find(val => val.MachineType == machineType);

        gameObject.LeanAlpha(0, 0);
        basePos = transform.position;
        transform.transform.position = new Vector2(basePos.x, basePos.y + 1.5f);

        // regist to env manager
        MainController.Instance.RegistGameState(this);

        MachineState = MachineState.OFF;

        EnvController.RegistMachine(this);
    }

    public void StartMachine()
    {
        print("Start machine");
        MachineState = MachineState.ON_IDDLE;
        StartCoroutine(ISpawn());
        InitStart();
    }

    IEnumerator ISpawn()
    {
        yield return 1;
        gameObject.LeanMoveLocalY(basePos.y, 1f);
        gameObject.LeanAlpha(1, 2f);
    }

    public abstract void InitStart();

    public void EnvInstance()
    {
        print("Sapwn");
    }

    public virtual void OnGameStateChanged(GameState _old, GameState _new) { }

    public virtual void OnMachineStateChanged(MachineState _old, MachineState _new) { }

    public void OnGameStateChanged()
    {
        boxCollider2D.enabled = GameState != GameState.IDDLE;
    }

    public void Command()
    {
        throw new System.NotImplementedException();
    }
}
