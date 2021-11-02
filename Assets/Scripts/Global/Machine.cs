using UnityEngine;
using Game;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Machine : MonoBehaviour, IGameState
{
    [Header("Propertie Machine")]
    public GameObject resultPrefab;         // Will depreceated
    public Transform resultSpawnPosition;   // Will depreceated
    public MachineIgrendient machineType;
    public Transform capacityBarPos;
    public Transform radiusBarPos;


    [Header("Debug")]
    public MachineData machineData;
    public MachineProperties machineProperties;
    public bool spawnOverlay = false;
    public int machineLevel = 0;

    [SerializeField] Vector2 basePos;
    [SerializeField] MachineState _machineState;
    [SerializeField] protected GameState gameState;
    [SerializeField] protected GameObject resultGO;
    [SerializeField] protected BoxCollider2D boxCollider2D;

    [Header("Component")]
    /** RADIUS BAR */
    [SerializeField] internal bool isUseRadiusBar = false;
    [SerializeField] internal BarMachine BarMachine;
    [SerializeField] internal GameObject BarMachineGO;

    /** CAPACITY */
    [SerializeField] internal bool isUseBarCapacity = false;
    [SerializeField] internal CapacityMachine CapacityMachine;
    [SerializeField] internal GameObject BarCapacityGO;

    /** OVERLAY */
    [SerializeField] internal bool isUseMachineOverlay = false;
    [SerializeField] internal MachineUI machineUI;

    /** OVERLAY */
    [SerializeField] internal bool isUseOverCook = false;
    //[SerializeField] internal MachineUI machineUI;

    #region FirstInit
    public void setMachineData(MachineData _machineData, int _machineLevel)
    {
        machineData = _machineData;
        machineType = _machineData.machineType;
        machineLevel = _machineLevel;

        if (_machineLevel >= _machineData.properties.Count)
        {
            _machineLevel = _machineData.properties.Count; // Prevent too much value
            Debug.LogWarning("Much Value");
        }
        machineProperties = machineData.properties[_machineLevel - 1];
    }

    private void OnEnable() => MainController.OnGameStateChanged += GameStateHandler;
    private void OnDisable() => MainController.OnGameStateChanged -= GameStateHandler;

    private void Awake() => boxCollider2D = GetComponent<BoxCollider2D>();

    private void Start()
    {
        MachineState = MachineState.INIT;
        gameObject.LeanAlpha(0, 0);                                 // Prevent machine render on first instance

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
            case MachineState.ON_OVERCOOK:
                OnMachineOverCook();
                break;
            case MachineState.ON_REPAIR:
                OnMachineRepair();
                break;
            default:
                break;
        }
    }

    public virtual void OnMachineRepair() { }

    public virtual void OnMachineOverCook() { }

    public virtual void OnMachineOff() { }

    public virtual void OnMachineInit() { }

    public virtual void OnMachineProcess()
    {
        baseAnimateOnProcess();
        if (isUseRadiusBar) BarMachine.runProgress(BarMachine.BarType.DEFAULT);
    }

    public virtual void OnMachineDone() { }

    public virtual void OnMachineClearance()
    {
        BarMachine.resetProgress();
    }

    public virtual void OnMachineIddle()
    {
        if (isUseRadiusBar) BarMachine.resetProgress();
    }

    #endregion

    public GameObject GetGameObject() => gameObject;

    public void baseAnimateOnProcess() => StartCoroutine(baseProcess());

    IEnumerator baseProcess()
    {
        LeanTween.scaleX(gameObject, .9f, .2f).setEaseInOutBounce().setLoopPingPong(5);
        LeanTween.scaleY(gameObject, .85f, .4f).setEaseInOutBounce().setLoopPingPong(5);
        yield return new WaitForSeconds(.4f);
    }

    public void useRadiusBar() => instanceRadiusBar();
    void instanceRadiusBar()
    {
        isUseRadiusBar = true;

        BarMachineGO = Instantiate(EnvController.Instance.radBarComponent, GameUIController.Instance.radiusUI);
        BarMachineGO.name = $"{gameObject.name}--radius-bar";
        BarMachineGO.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(radiusBarPos.position.x, radiusBarPos.position.y, 0));

        BarMachine = BarMachineGO.GetComponent<BarMachine>();
        BarMachine.init(this);
    }

    public void barMachineDone()
    {
        MachineState = MachineState.ON_DONE;

        if (isUseBarCapacity)
        {
            CapacityMachine.setFull();
            BarMachine.resetProgress();
        }
    }

    #region Bar Capacity

    public void useBarCapacity() => instanceBarCapacity();
    void instanceBarCapacity()
    {
        isUseBarCapacity = true;

        BarCapacityGO = Instantiate(EnvController.Instance.capacityBarComponent, GameUIController.Instance.capacityUI);
        BarCapacityGO.name = $"{gameObject.name}--capacity-bar";
        BarCapacityGO.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(capacityBarPos.position.x, capacityBarPos.position.y, 0));

        CapacityMachine = BarCapacityGO.GetComponent<CapacityMachine>();
        CapacityMachine.init(this);
    }

    internal void emptyCapacity()
    {
        MachineState = MachineState.ON_IDDLE;
    }

    #endregion

    #region Spawn Overlay

    public void useMachineOverlay() => registUIOverlay();

    void registUIOverlay()
    {
        isUseMachineOverlay = true;
        GameUIController.Instance.machineOverlay.registMachine(this, out machineUI);
    }

    #endregion

    #region OVERCOOK

    public void useOverCook()
    {
        isUseOverCook = true;
    }

    public void initOverCook()
    {
        print("Over cook on going");
        BarMachine.runProgress(BarMachine.BarType.OVERCOOK);
        MachineState = MachineState.ON_OVERCOOK;
    }

    #endregion

    #region REPAIR

    public void initRepair()
    {
        MachineState = MachineState.ON_REPAIR;
        print("Machine Need Repair");
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

    IEnumerator ISpawn()
    {
        yield return 1;
        gameObject.LeanMoveLocalY(basePos.y, GlobalController.Instance.startingAnimLenght / 2);
        gameObject.LeanAlpha(1, GlobalController.Instance.startingAnimLenght);
    }

}
