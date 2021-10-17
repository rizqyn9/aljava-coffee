using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MachineUI : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField] MachineData machineData;

        public void setMachineData(MachineData _machineData) => machineData = _machineData;
    }
}
