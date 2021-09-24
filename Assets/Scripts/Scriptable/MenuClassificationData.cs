using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Classification Menu", menuName = "ScriptableObject/MenuClassification")]
public class MenuClassificationData : ScriptableObject
{
    public MenuClassification MenuClassification;
    public List<MachineType> ListMachine;
}
