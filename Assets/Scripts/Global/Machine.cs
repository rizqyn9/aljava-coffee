using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Machine : MonoBehaviour
{
    [Header("Propertie Machine")]
    public GameObject resultPrefab;
    public Transform resultSpawnPosition;
    public enumMachineType machineType;
    public enumMachineType nextTargetMachine;

    [Header("Debug")]
    public enumMachineState enumMachineState;
    public GameObject resultGO;

    private void Start()
    {
        RegistToManager();
    }

    public abstract void RegistToManager();

    

}
