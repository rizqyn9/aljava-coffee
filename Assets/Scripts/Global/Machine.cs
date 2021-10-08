using UnityEngine;
using Game;
using System.Collections;
using System;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Machine : MonoBehaviour, IGameState
{
    [Header("Propertie Machine")]
    public GameObject resultPrefab;
    public Transform resultSpawnPosition;
    public MachineIgrendient machineType;

    [Header("Debug")]
    public MachineData MachineData;
    [SerializeField] Vector2 basePos;
    [SerializeField] MachineState _machineState;
    [SerializeField] protected GameState gameState;
    [SerializeField] protected GameObject resultGO;
    [SerializeField] protected BoxCollider2D boxCollider2D;
    [SerializeField] protected BarMachine BarMachine;
    [SerializeField] protected GameObject RadiusBar;

    private void OnEnable() => MainController.OnGameStateChanged += GameStateHandler;
    private void OnDisable() => MainController.OnGameStateChanged -= GameStateHandler;

    #region GAME STATE
    public void GameStateHandler(GameState _gameState)
    {
        //print("====== Machine Game state handler");
        gameState = _gameState;
        GameStateController.UpdateGameState(this, _gameState);
        OnGameStateChanged();
    }

    public virtual void OnGameStateChanged() { }

    // Access from child
    public virtual void OnGameIddle()
    {
        boxCollider2D.enabled = false;
    }

    public virtual void OnGameBeforeStart()
    {
        StartCoroutine(ISpawn());
    }

    public virtual void OnGameStart()
    {
        boxCollider2D.enabled = true;
        MachineState = MachineState.ON_IDDLE;
    }
    public virtual void OnGamePause() { }
    public virtual void OnGameClearance() { }
    public virtual void OnGameFinish() { }
    public virtual void OnGameInit()
    {
        boxCollider2D.enabled = false;
    }
    #endregion

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        gameObject.LeanAlpha(0, 0);
        //transform.transform.position = new Vector2(basePos.x, basePos.y + 1.5f);
        basePos = transform.position;

        gameState = MainController.GameState;           // To ensure that this variable sync on Main Controller
        GameStateController.UpdateGameState(this, gameState);      //

        EnvController.RegistMachine(this);

        MachineState = MachineState.OFF;
    }

    #region MACHINE STATE
    public MachineState MachineState
    {
        get => _machineState;
        set
        {
            if (_machineState == value) return;
            _machineState = value;
            OnMachineStateChanged(_machineState, value);
        }
    }

    public virtual void OnMachineStateChanged(MachineState _old, MachineState _new)
    {
        switch (_machineState)
        {
            case MachineState.OFF:
                OnMachineOff();
                break;
            case MachineState.INIT:
                OnMachineInit();
                break;
            case MachineState.ON_IDDLE:
                OnMachineIddle();
                break;
            case MachineState.ON_PROCESS:
                OnMachineProcess();
                break;
            case MachineState.ON_DONE:
                OnMachineDone();
                break;
            case MachineState.ON_CLEARANCE:
                OnMachineClearance();
                break;
            default:
                break;
        }
    }

    public virtual void OnMachineOff() { }

    public virtual void OnMachineInit() { }

    public virtual void OnMachineDone() { }

    public virtual void OnMachineProcess() { }

    public virtual void OnMachineClearance() { }

    public virtual void OnMachineIddle() { }

    #endregion

    public void SetMachineData(MachineData _machineData)
    {
        MachineData = _machineData;
        machineType = _machineData.MachineType;
    }

    IEnumerator ISpawn()
    {
        yield return 1;
        gameObject.LeanMoveLocalY(basePos.y, 1f);
        gameObject.LeanAlpha(1, 2f);
    }

    public GameObject GetGameObject() => gameObject;

    public void baseAnimateOnProcess()
    {
        LeanTween.scaleX(gameObject, .9f, .2f).setEaseInOutBounce().setLoopPingPong(5);
        LeanTween.scaleY(gameObject, .85f, .4f).setEaseInOutBounce().setLoopPingPong(5);
    }

    public void instanceRadiusBar()
    {
        RadiusBar = Instantiate(EnvController.BarComponent, FindObjectOfType<Canvas>().transform);
        RadiusBar.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        BarMachine = RadiusBar.GetComponent<BarMachine>();
        BarMachine.time = MachineData.durationProcess;
    }
}
