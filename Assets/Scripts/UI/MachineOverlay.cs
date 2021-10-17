using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MachineOverlay : MonoBehaviour
    {
        [SerializeField] bool isOverlayActive = false;
        [SerializeField] Vector2 startPos;

        public bool reqOverlay()
        {
            if (isOverlayActive) return false;

            startPos = transform.position;

            setOverlayActive();
            return true;
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
            gameObject.LeanMoveLocalY(startPos.y - 100, 1f).setEaseInOutBack();
            isOverlayActive = false;
        }

        public void BTN_CloseOverlay()
        {
            setOverlayNonActive();
        }
    }
}
