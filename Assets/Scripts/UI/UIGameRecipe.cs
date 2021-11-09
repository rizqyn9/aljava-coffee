using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class UIGameRecipe : MonoBehaviour
    {
        [Header("Properties")]
        public bool isActive;

        public float move;
        public void Btn_Slider()
        {
            LeanTween.moveX(GetComponent<RectTransform>(), isActive ? 0 : -300f, .7f);
            GameUIController.Instance.setNoClickArea(!isActive);
            isActive = !isActive;
        }
    }
}
