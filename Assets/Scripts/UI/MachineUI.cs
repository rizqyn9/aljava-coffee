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

        public void init(Machine _machine, MachineOverlay _machineOverlay)
        {
            machine = _machine;
            machineOverlay = _machineOverlay;
            gameObject.SetActive(false);
        }

        public bool reqInstance()
        {
            if (machineOverlay.isOverlayActive) return false;

            gameObject.SetActive(true);
            machineOverlay.setOverlayActive();
            return true;
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
            machine.spawnOverlay = !isApproved;
        }

        public virtual void OnApprovalChange() { }


    }
}
