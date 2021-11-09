using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class CoffeeMaker : Machine
    {
        [Header("Properties")]
        public Color colorIgrendientsOutput;

        [Header("Debug")]
        [SerializeField] GlassRegistered glassTarget;
        [SerializeField] List<MachineIgrendient> igrendientsList = new List<MachineIgrendient>();

        private void OnMouseDown()
        {
            if ((MachineState == MachineState.ON_DONE || MachineState == MachineState.ON_OVERCOOK)
                && GlassContainer.IsGlassTargetAvaible(MachineIgrendient.NULL, out glassTarget)
                && igrendientsList.Count >= 0
                )
            {
                OnMachineValidate();
            }
            if(MachineState == MachineState.ON_NEEDREPAIR)
            {
                MachineState =MachineState.ON_REPAIR;
            }
        }
        public override void OnMachineIddle()
        {
            base.OnMachineIddle();
            igrendientsList = new List<MachineIgrendient>();
        }

        public override void reqInput(MachineIgrendient _MachineIgrendient)
        {
            MachineState = MachineState.ON_PROCESS;

            igrendientsList.Add(_MachineIgrendient);
            igrendientsList.Add(machineData.machineType);
        }

        public override void validateLogic()
        {
            Sprite _sprite = getSprite(igrendientsList[0]);
            glassTarget.glass.changeSpriteAddIgrendients(_sprite, _multipleIgrendients: igrendientsList);
            glassTarget.glass.process();

            resetDepends();
            barMachine.stopBar();
            MachineState = MachineState.ON_IDDLE;
        }

        void resetDepends()
        {
            igrendientsList = new List<MachineIgrendient>();
        }
    }
}
