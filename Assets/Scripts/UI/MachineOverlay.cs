using UnityEngine;

namespace Game
{
    public class MachineOverlay : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] float yOffset;

        [Header("Debug")]
        [SerializeField] bool isOverlayActive = false;
        [SerializeField] MachineData machineData;
        [SerializeField] MachineUI machineUI;
        [SerializeField] GameObject spawnGO;

        private void OnEnable()
        {
            transform.position = new Vector2(transform.position.x, yOffset);
        }

        public bool reqOverlay(MachineData _machineData)
        {
            if (isOverlayActive) return false;

            spawnGO = Instantiate(_machineData.PrefabUIOverlay, gameObject.transform);
            machineUI = spawnGO.GetComponent<MachineUI>();
            machineData = _machineData;

            setOverlayActive();
            return true;
        }

        public void reset()
        {

        }

        void setOverlayActive()
        {
            if (isOverlayActive) return ;
            gameObject.LeanMoveLocalY(0, 1f).setEaseInOutBack();
            isOverlayActive = true;
        }

        void setOverlayNonActive()
        {
            if (!isOverlayActive) return;
            gameObject.LeanMoveLocalY(yOffset, 1f).setEaseInOutBack();
            isOverlayActive = false;
        }

        public void BTN_CloseOverlay()
        {
            setOverlayNonActive();
        }
    }
}
