using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Classification Menu", menuName = "ScriptableObject/MenuClassification")]
public class MenuClassificationData : ScriptableObject
{
    public List<MachineClass> MachineClass;
    public List<MenuClassification> MenuClassification;
    public List<MachineType> ListMachine;
    public GameObject prefabManager;
}
