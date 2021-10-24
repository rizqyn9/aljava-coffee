using UnityEngine;

namespace Game
{
    public class MachineOverlay : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] float yOffset;
        [SerializeField] GameObject closeBtn;

        [Header("Debug")]
        [SerializeField] bool isOverlayActive = false;
        [SerializeField] Machine machine;
        [SerializeField] MachineData machineData;
        [SerializeField] MachineUI machineUI;
        [SerializeField] GameObject spawnGO;

        private void OnEnable() => defaultPosition();

        public void defaultPosition()
        {
            transform.position = new Vector2(transform.position.x, yOffset);
        }

        public bool reqOverlay(Machine _machine)
        {
            if (isOverlayActive) return false;

            machineData = _machine.MachineData;
            spawnGO = Instantiate(machineData.PrefabUIOverlay, gameObject.transform);
            machineUI = spawnGO.GetComponent<MachineUI>();
            machineUI.setData(_machine, this);

            setOverlayActive();
            return true;
        }


        void setOverlayActive()
        {
            if (isOverlayActive) return ;
            GameUIController.Instance.setNoClickArea(true);
            gameObject.LeanMoveLocalY(0, 1f).setEaseInOutBack();
            isOverlayActive = true;
        }

        void setOverlayNonActive()
        {
            if (!isOverlayActive) return;
            GameUIController.Instance.setNoClickArea(false);
            gameObject.LeanMoveLocalY(yOffset, 1f).setEaseInOutBack();
            isOverlayActive = false;
        }

        public void BTN_CloseOverlay()
        {
            setOverlayNonActive();
        }

        #region MachineUI Controller

        public void handleApprove(bool isApprove)
        {
            if (isApprove)
            {
                setOverlayNonActive();
            }
            else
            {
                Debug.LogWarning("rejected");
            }
        }

        #endregion
    }
}
