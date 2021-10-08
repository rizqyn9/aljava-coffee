using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public bool autoRun = false;
    public GameObject PrefabManager;
    public GameObject PrefabResult;
    [Tooltip("Instance UI overlay when machine touched on first time")]
    public GameObject PrefabUIOverlay;
    public float durationProcess = .5f;
    public bool useBarComponent = true;
}

#if UNITY_EDITOR
[CustomEditor(typeof(MachineData))]
public class editScript : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.LabelField("haha");
        GUILayout.Button("asdsad");
    }
}
#endif