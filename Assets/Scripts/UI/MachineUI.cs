using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class MachineUI : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField] Machine machine;
        [SerializeField] bool isApproved;
        [SerializeField] MachineOverlay machineOverlay;

        public void setData(Machine _machine, MachineOverlay _machineOverlay)
        {
            machine = _machine;
            machineOverlay = _machineOverlay;
        }


        public void handleApprovalChange(bool res)
        {
            isApproved = res;
            machineOverlay.handleApprove(res);
            OnApprovalChange();
        }

        public virtual void OnApprovalChange() { }


    }
}
