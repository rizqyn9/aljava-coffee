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

    private void OnEnable() => MainController.OnGameStateChanged += GameStateHandler;
    private void OnDisable() => MainController.OnGameStateChanged -= GameStateHandler;

    #region GAME STATE
    public void GameStateHandler(GameState _gameState)
    {
        //print("====== Machine Game state handler");
        gameState = _gameState;
        GameStateController.UpdateGameState(this, _gameState);
    }

    // Access from child
    public virtual void OnGameIddle() { }

    public virtual void OnGameBeforeStart()
    {
        StartCoroutine(ISpawn());
    }

    public virtual void OnGameStart()
    {
        MachineState = MachineState.ON_IDDLE;
    }
    public virtual void OnGamePause() { }
    public virtual void OnGameClearance() { }
    public virtual void OnGameFinish() { }
    public virtual void OnGameInit() { }
    #endregion

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        gameObject.LeanAlpha(0, 0);
        //transform.transform.position = new Vector2(basePos.x, basePos.y + 1.5f);
        //basePos = transform.position;

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
            //print("Machine State");
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
            case MachineState.ON_IDDLE:
                OnMachineIddle();
                break;
            case MachineState.ON_PROCESS:
                OnMachineProcess();
                break;
            case MachineState.ON_DONE:
                OnMachineDone();
                break;
            default:
                break;
        }
    }

    public virtual void OnMachineOff() { }

    public virtual void OnMachineDone() { }

    public virtual void OnMachineProcess() { }

    public virtual void OnMachineIddle() { }


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


    public virtual void InitStart() { }

    public GameObject GetGameObject() => gameObject;

    public void baseAnimateOnProcess()
    {
        LeanTween.scaleX(gameObject, .9f, .2f).setEaseInOutBounce().setLoopPingPong(5);
        LeanTween.scaleY(gameObject, .85f, .4f).setEaseInOutBounce().setLoopPingPong(5);
    }
}
