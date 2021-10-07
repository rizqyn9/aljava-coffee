using UnityEngine;
using Game;
using System;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Machine : MonoBehaviour, IGameState
{
    [Header("Propertie Machine")]
    public GameObject resultPrefab;
    public Transform resultSpawnPosition;
    public MachineIgrendient machineType;
    public IGameState asd;

    [Header("Debug")]
    public MachineData MachineData;
    [SerializeField] Vector2 basePos;
    [SerializeField] MachineState _machineState;
    [SerializeField] protected GameState gameState;
    [SerializeField] protected GameObject resultGO;
    [SerializeField] protected BoxCollider2D boxCollider2D;

    private void OnEnable()
    {
        MainController.OnGameStateChanged += GameStateHandler;
    }

    private void OnDisable()
    {
        MainController.OnGameStateChanged -= GameStateHandler;
    }

    #region GAME STATE
    public void GameStateHandler(GameState _gameState)
    {
        
    }

    // Access from child
    public virtual void OnGameIddle()
    {
        print("Machine On Iddle");
    }
    public virtual void OnGameBeforeStart()
    {
        //print("Machine On Before Start;");
    }
    public virtual void OnGameStart()
    {
        print("Machine On Start");
    }
    public virtual void OnGamePause() { }
    public virtual void OnGameClearance() { }
    public virtual void OnGameFinish() { }

    #endregion

    private void Start()
    {
        gameObject.LeanAlpha(0, 0);
        transform.transform.position = new Vector2(basePos.x, basePos.y + 1.5f);
        basePos = transform.position;

        gameState = MainController.GameState;           // To ensure that this variable sync on Main Controller
        GameStateController.UpdateGameState(this, gameState);      //

        EnvController.RegistMachine(this);
        //MachineData = LevelController.Instance.MachineDatas.Find(val => val.MachineType == machineType);

        MachineState = MachineState.OFF;
    }

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


    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void SetMachineData(MachineData _machineData)
    {
        MachineData = _machineData;
        machineType = _machineData.MachineType;
    }

    public void StartMachine()
    {
        //print("Start machine");
        //MachineState = MachineState.ON_IDDLE;
        //StartCoroutine(ISpawn());
        //InitStart();
    }

    IEnumerator ISpawn()
    {
        yield return 1;
        gameObject.LeanMoveLocalY(basePos.y, 1f);
        gameObject.LeanAlpha(1, 2f);
    }

    public virtual void InitStart() { }

    public virtual void OnMachineStateChanged(MachineState _old, MachineState _new) { }

    public GameObject GetGameObject() => gameObject;

    public void OnGameInit()
    {
    }
}
