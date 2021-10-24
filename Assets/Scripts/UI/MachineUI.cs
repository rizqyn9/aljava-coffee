using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class MachineUI : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] bool useCheckButton = true;

        [Header("Debug")]
        [SerializeField] Machine machine;
        [SerializeField] MachineOverlay machineOverlay;

        public void setData(Machine _machine, MachineOverlay _machineOverlay)
        {
            machine = _machine;
            machineOverlay = _machineOverlay;
        }


        [SerializeField] bool _isApproved;
        public bool isApproved
        {
            get => _isApproved;
            set
            {
                _isApproved = value;
                OnApprovalChange();
                if (!useCheckButton) handleCheck();
            }
        }

        public void handleCheck()
        {
            machineOverlay.handleApprove(isApproved);
        }

        public virtual void OnApprovalChange() { }


    }
}
