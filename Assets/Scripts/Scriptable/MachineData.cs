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
    public GameObject PrefabManager;
    public GameObject PrefabResult;

    [Tooltip("Instance UI overlay when machine touched on first time")]
    public bool useUIOverlay = true;
    public GameObject PrefabUIOverlay;

    [Header("Duration Component")]
    public bool autoRun = false;
    public Vector2 posBarDuration = new Vector2(1, 1);
    public float durationProcess = .5f;
    //public bool useBarComponent = true;

    [Header("Capacity")]
    public Vector2 posBarCapacity = new Vector2(-.5f, .6f);
    public bool useBarCapacity = true;
    public int maxCapacity = 2;
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