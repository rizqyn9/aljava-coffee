using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

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
    public bool isGameStarted = false;
    [SerializeField] protected GameObject resultGO;
    [SerializeField] protected BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        RegistToManager();
    }

    public void startMachine() => InitStart();

    public abstract void RegistToManager();

    public abstract void InitStart();
}
