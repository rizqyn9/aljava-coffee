using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class UI_BeansMachine : MachineUI
    {
        [SerializeField] BeansMachine beansMachine;

        public void Btn_SimApprove() => handleApprovalChange(true);
        public void Btn_SimReject() => handleApprovalChange(false);
    }
}
