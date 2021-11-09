using UnityEngine;
using Game;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Machine : MonoBehaviour, IGameState
{
    [Header("Propertie Machine")]
    public GameObject resultPrefab;         // Will depreceated
    public Transform resultSpawnPosition;   // Will depreceated
    public MachineIgrendient machineType;
    public Transform capacityBarPos;
    public Transform radiusBarPos;
    public List<SpriteGlassState> spriteGlassStates;

    [Header("Debug")]
    public MachineData machineData;
    public MachineProperties machineProperties;
    public bool spawnOverlay = false;
    public int machineLevel = 0;

    [SerializeField] Vector2 basePos;
    [SerializeField] MachineState _machineState;
    [SerializeField] protected GameState gameState;
    [SerializeField] protected GameObject resultGO;
    public BoxCollider2D boxCollider2D;

    [Header("Component")]
    /** RADIUS BAR */
    [SerializeField] internal bool isUseRadiusBar = false;
    [SerializeField] internal BarMachine barMachine;
    [SerializeField] internal GameObject BarMachineGO;

    /** CAPACITY */
    [SerializeField] internal bool isUseBarCapacity = false;
    [SerializeField] internal CapacityMachine capacityMachine;
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

        if ((_machineLevel - 1) >= _machineData.properties.Count) // Prevent too much value
        {
            _machineLevel = _machineData.properties.Count; 
            Debug.LogWarning($"{gameObject.name} Much Value");
        }
        machineProperties = machineData.properties[_machineLevel - 1];

        setUpComponent();
    }

    private void OnEnable() => MainController.OnGameStateChanged += GameStateHandler;
    private void OnDisable() => MainController.OnGameStateChanged -= GameStateHandler;

    private void Awake() => boxCollider2D = GetComponent<BoxCollider2D>();

    private void Start()
    {
        MachineState = MachineState.INIT;
        gameObject.LeanAlpha(0, 0);                                 // Prevent machine render on first instance
        setColliderDisabled();

        basePos = transform.position;

        gameState = MainController.GameState;                       // To ensure that this variable sync on Main Controller
        GameStateController.UpdateGameState(this, gameState);       

        EnvController.RegistMachine(this);

        MachineState = MachineState.OFF;
    }
    #endregion

    #region
    public void setColliderEnabled() => boxCollider2D.enabled = true;
    public void setColliderDisabled() => boxCollider2D.enabled = false;
    #endregion

    #region GAME STATE
    public GameObject GetGameObject() => gameObject;

    public void GameStateHandler(GameState _gameState)
    {
        gameState = _gameState;
        GameStateController.UpdateGameState(this, _gameState);
        OnGameStateChanged();
    }
    public virtual void OnGameStateChanged() { }

    public virtual void OnGameBeforeStart() => StartCoroutine(ISpawn());

    public virtual void OnGameStart() => MachineState = MachineState.ON_IDDLE;

    public virtual void OnGameIddle() { }
    public virtual void OnGamePause() { }
    public virtual void OnGameClearance() { }
    public virtual void OnGameFinish() { }
    public virtual void OnGameInit() { }
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
            case MachineState.INIT:
                OnMachineInit();
                break;
            case MachineState.OFF:
                OnMachineOff();
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
            case MachineState.ON_NEEDREPAIR:
                OnMachineNeedRepair();
                break;
            default:
                break;
        }
    }

    public virtual void OnMachineInit()
    {
        setColliderDisabled();
    }

    public virtual void OnMachineOff()
    {
        setColliderDisabled();
    }

    public virtual void OnMachineIddle()
    {
        if (isUseRadiusBar) barMachine.stopBar(); ;
        setColliderEnabled();
    }

    public virtual void OnMachineProcess()
    {
        //baseAnimateOnProcess();
        setColliderDisabled();
        if (isUseRadiusBar) barMachine.runProgress(BarMachine.BarType.DEFAULT);
    }

    public virtual void OnMachineDone()
    {
        setColliderEnabled();
    }

    public virtual void OnMachineClearance()
    {
        barMachine.resetProgress();
    }

    /// <summary>
    /// Call every user try to decrement capacity machine
    /// </summary>
    public virtual void OnMachineServe()
    {
        if (isUseRadiusBar) barMachine.resetProgress();
        if (isUseBarCapacity)
            capacityMachine.getOne();
        print("Machine Serve");
    }

    public virtual void OnMachineSpawn()
    {
        MachineState = MachineState.ON_PROCESS;
    }

    public virtual void OnMachineValidate()
    {
        setColliderDisabled();
        if (isUseOverCook)
        {
            barMachine.stopBar();
        }
        validateLogic();
        setColliderEnabled();
    }

    public virtual void OnMachineRepair()
    {
        barMachine.simulateRepair();
    }
    public virtual void OnMachineNeedRepair() { }

    public virtual void OnMachineOverCook() { }

    public virtual void OnMachineSpawnOverlay()
    {
        machineUI.reqInstance();
    }

    public virtual void validateLogic() { }

    #endregion

    #region RadialBar

    public void useRadiusBar(bool _isUse)
    {
        isUseRadiusBar = _isUse;
        if (!isUseRadiusBar) return;

        BarMachineGO = Instantiate(EnvController.Instance.radBarComponent, GameUIController.Instance.radiusUI);
        barMachine = BarMachineGO.GetComponent<BarMachine>();
        barMachine.init(this);
    }

    public void barMachineDone()
    {
        MachineState = MachineState.ON_DONE;

        if (isUseBarCapacity)
        {
            capacityMachine.setFull();
            barMachine.resetProgress();
        }
    }

    #endregion

    #region OVERCOOK

    public void useOverCook(bool _isUse) => isUseOverCook = _isUse;

    public IEnumerator I_InitOverCook()
    {
        print("Over cook on going");
        MachineState = MachineState.ON_OVERCOOK;
        yield return new WaitForSeconds(1f);
        if(MachineState == MachineState.ON_OVERCOOK)
        {
            barMachine.runProgress(BarMachine.BarType.OVERCOOK);
        }
        yield break;
    }

    #endregion

    #region Bar Capacity

    public void useBarCapacity(bool _isUse)
    {
        isUseBarCapacity = _isUse;
        if (!isUseBarCapacity) return;

        BarCapacityGO = Instantiate(EnvController.Instance.capacityBarComponent, GameUIController.Instance.capacityUI);
        BarCapacityGO.name = $"{gameObject.name}--capacity-bar";
        BarCapacityGO.transform.position = Camera.main.WorldToScreenPoint(capacityBarPos.position);

        capacityMachine = BarCapacityGO.GetComponent<CapacityMachine>();
        capacityMachine.init(this);
    }

    internal void emptyCapacity()
    {
        MachineState = MachineState.ON_IDDLE;
    }

    #endregion

    #region Spawn Overlay

    public void useMachineOverlay(bool _isUse)
    {
        isUseMachineOverlay = _isUse;
        if (!isUseMachineOverlay) return;

        spawnOverlay = true;
        isUseMachineOverlay = true;
        GameUIController.Instance.machineOverlay.registMachine(this, out machineUI);
    }

    #endregion

    #region REPAIR

    public void initRepair()
    {
        MachineState = MachineState.ON_NEEDREPAIR;
        print("Machine Need Repair");
    }

    public virtual void OnMachineSuccessRepair()
    {
        MachineState = MachineState.ON_IDDLE;
    }

    #endregion

    #region Dependency

    public Sprite getSprite(MachineIgrendient machineIgrendient)
    {
        return spriteGlassStates.Find(val => val.igrendient == machineIgrendient).sprite;
    }

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

    /// <summary>
    /// Init set up to instance an depends 
    /// </summary>
    private void setUpComponent()
    {
        useRadiusBar(machineData.isUseRadiusBar);
        useOverCook(machineData.isUseOverCook);
        useBarCapacity(machineData.isUseBarCapacity);
        useMachineOverlay(machineData.isUseMachineOverlay);
    }

    public virtual void reqInput(MachineIgrendient _machineIgrendient) { }
    public virtual void reqInput(MachineIgrendient _machineIgrendient, Machine _machine) { }
    public virtual void reqInput(List<MachineIgrendient> _listMachineIgrendient) { }

    #endregion

    #region Depreceated

    IEnumerator ISpawn()
    {
        yield return 1;
        gameObject.LeanMoveLocalY(basePos.y, GlobalController.Instance.startingAnimLenght / 2);
        gameObject.LeanAlpha(1, GlobalController.Instance.startingAnimLenght);
    }

    //public void baseAnimateOnProcess() => StartCoroutine(baseProcess());

    //IEnumerator baseProcess()
    //{
    //    LeanTween.scaleX(gameObject, .9f, .2f).setEaseInOutBounce().setLoopPingPong(5);
    //    LeanTween.scaleY(gameObject, .85f, .4f).setEaseInOutBounce().setLoopPingPong(5);
    //    yield return new WaitForSeconds(.4f);
    //}

    #endregion
}
