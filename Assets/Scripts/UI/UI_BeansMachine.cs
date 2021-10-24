using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class UI_BeansMachine : MachineUI
    {
        public void Btn_SimApprove() => isApproved = true;
        public void Btn_SimReject() => isApproved = false;
        public void Btn_Check() => handleCheck();
    }
}
