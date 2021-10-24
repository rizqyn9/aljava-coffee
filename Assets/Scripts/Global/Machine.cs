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
    public Transform posProgressBar;
    public Transform posBarCapacity;

    [Header("Debug")]
    public MachineData MachineData;
    [SerializeField] Vector2 basePos;
    [SerializeField] MachineState _machineState;
    [SerializeField] protected GameState gameState;
    [SerializeField] protected GameObject resultGO;
    [SerializeField] protected BoxCollider2D boxCollider2D;

    /** RADIUS BAR */
    [SerializeField] protected BarMachine BarMachine;
    [SerializeField] protected GameObject BarMachineGO;

    /** CAPACITY */
    [SerializeField] protected CapacityMachine CapacityMachine;
    [SerializeField] internal GameObject BarCapacityGO;

    #region FirstInit
    public void SetMachineData(MachineData _machineData)
    {
        MachineData = _machineData;
        machineType = _machineData.MachineType;
    }

    private void OnEnable() => MainController.OnGameStateChanged += GameStateHandler;
    private void OnDisable() => MainController.OnGameStateChanged -= GameStateHandler;
    private void Awake() => boxCollider2D = GetComponent<BoxCollider2D>();

    private void Start()
    {
        MachineState = MachineState.INIT;
        gameObject.LeanAlpha(0, 0);

        basePos = transform.position;

        gameState = MainController.GameState;                       // To ensure that this variable sync on Main Controller
        GameStateController.UpdateGameState(this, gameState);       //

        EnvController.RegistMachine(this);

        MachineState = MachineState.OFF;
    }
    #endregion

    #region GAME STATE
    public void GameStateHandler(GameState _gameState)
    {
        gameState = _gameState;
        GameStateController.UpdateGameState(this, _gameState);
        OnGameStateChanged();
    }

    public virtual void OnGameStateChanged() { }

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

    public virtual void OnMachineInit()
    {
        instanceRadiusBar();
        if (MachineData.useBarCapacity)
        {
            instanceBarCapacity();
        }
    }

    public virtual void OnMachineProcess()
    {
        baseAnimateOnProcess();
        BarMachine.isActive = true;
    }

    public virtual void OnMachineDone()
    {

    }

    public virtual void OnMachineClearance()
    {
        BarMachine.isActive = false;
    }
    public virtual void OnMachineIddle() { }

    #endregion

    IEnumerator ISpawn()
    {
        yield return 1;
        gameObject.LeanMoveLocalY(basePos.y, GlobalController.Instance.startingAnimLenght/2);
        gameObject.LeanAlpha(1, GlobalController.Instance.startingAnimLenght);
    }

    public GameObject GetGameObject() => gameObject;

    public void baseAnimateOnProcess()
    {
        LeanTween.scaleX(gameObject, .9f, .2f).setEaseInOutBounce().setLoopPingPong(5);
        LeanTween.scaleY(gameObject, .85f, .4f).setEaseInOutBounce().setLoopPingPong(5);
    }

    void instanceRadiusBar()
    {
        BarMachineGO = Instantiate(EnvController.Instance.radBarComponent, GameUIController.Instance.radiusUI);
        BarMachineGO.name = $"{gameObject.name}--radius-bar";
        BarMachineGO.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(MachineData.posBarDuration.x, MachineData.posBarDuration.y, 0));
        BarMachine = BarMachineGO.GetComponent<BarMachine>();
        BarMachine.machine = this;
        BarMachine.time = MachineData.durationProcess;
    }

    #region Bar Capacity
    void instanceBarCapacity()
    {
        BarCapacityGO = Instantiate(EnvController.Instance.capacityBarComponent, GameUIController.Instance.capacityUI);
        BarCapacityGO.name = $"{gameObject.name}--capacity-bar";
        BarCapacityGO.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(MachineData.posBarCapacity.x, MachineData.posBarCapacity.y, 0));

        CapacityMachine = BarCapacityGO.GetComponent<CapacityMachine>();
        CapacityMachine.init(this);
    }


    #endregion


    /// <summary>
    /// for validating when player try to click gameObject
    /// </summary>
    /// <returns></returns>
    public virtual bool isInteractable()
    {
        if (
            !MainController.Instance.onUI
            && gameState != GameState.START
            ) return false;
        else return true;
    }
}
