using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "MachineData", menuName = "ScriptableObject/MachineData")]
public class MachineData : ScriptableObject
{
    public MachineClass MachineClass;
    public MachineIgrendient MachineType;
    public MachineIgrendient TargetMachine;
    public bool autoRun = false;
    public GameObject PrefabManager;
    public GameObject PrefabResult;
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