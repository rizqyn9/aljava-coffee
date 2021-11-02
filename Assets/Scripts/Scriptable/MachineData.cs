using UnityEngine;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// TODO
/// Create levelling Machine
/// Dynamic value for duration long time
/// </summary>
[CreateAssetMenu(fileName = "MachineData", menuName = "ScriptableObject/MachineData")]
public class MachineData : ScriptableObject
{
    public MachineClass machineClass;
    public MachineIgrendient machineType;
    public MachineIgrendient targetMachine;
    public GameObject basePrefab;
    public GameObject PrefabResult;

    [Tooltip("Instance UI overlay when machine touched on first time")]
    public bool isUseMachineOverlay = false;
    public GameObject prefabUIOverlay;

    [Header("Component")]
    public bool isUseRadiusBar = false;
    public bool isUseBarCapacity = false;

    [Header("Machine Properties")]
    public bool isUpgradeable = true;
    public bool isAutoRun = false;
    public List<MachineProperties> properties = new List<MachineProperties>();
}

#if UNITY_EDITOR
[CustomEditor(typeof(MachineData))]
public class editScript : Editor
{
    public MachineData machineData;
    public Machine machine;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        machineData = (MachineData)target;

        EditorGUILayout.LabelField("Validate");
        if(GUILayout.Button("Validate " + target.name))
        {
            if (validateData())
            {
                Debug.Log("Once");
            }
        }
    }

    private bool validateData()
    {
        // Validate use case component
        try
        {
            machine = machineData.basePrefab.GetComponent<Machine>();

            if (
                machineData.properties.Count <= 0
                || (machineData.isUpgradeable && machineData.properties.Count == 1)
                || (!machineData.isUpgradeable && machineData.properties.Count > 1)
                || (machineData.isUseMachineOverlay && machineData.prefabUIOverlay == null)
                ) throw new Exception("Properties Rejected");

            foreach(MachineProperties properties in machineData.properties)
            {
                if(machineData.isUseBarCapacity && properties.maxCapacity == 0)
                {
                    throw new Exception("Max Capacity");
                }
                if(machineData.isUseRadiusBar && properties.processDuration == 0 )
                {
                    throw new Exception("Processing time error Capacity");
                }

            }

            return true;

        } catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }

    }
}
#endif