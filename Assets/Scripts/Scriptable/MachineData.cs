using UnityEngine;
using System.Collections.Generic;
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
    public MachineClass MachineClass;
    public MachineIgrendient MachineType;
    public MachineIgrendient TargetMachine;
    public GameObject PrefabManager;
    public GameObject PrefabResult;

    [Tooltip("Instance UI overlay when machine touched on first time")]
    public GameObject PrefabUIOverlay;

    [Header("Duration Component")]
    public bool autoRun = false;
    public Vector2 posBarDuration = new Vector2(1, 1);
    public float durationProcess = .5f;

    [Header("Capacity")]
    public Vector2 posBarCapacity = new Vector2(-.5f, .6f);
    public bool useBarCapacity = true;
    public int maxCapacity = 2;

    [Header("Machine Properties")]
    public bool isUpgradeable = true;
    public List<MachineProperties> properties = new List<MachineProperties>();
}

#if UNITY_EDITOR
[CustomEditor(typeof(MachineData))]
public class editScript : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.LabelField("Validate");
        GUILayout.Button("Validate Data");
    }
}
#endif