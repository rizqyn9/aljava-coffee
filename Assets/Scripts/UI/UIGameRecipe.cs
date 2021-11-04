using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class UIGameRecipe : MonoBehaviour
    {
        [Header("Properties")]
        public bool isActive;

        //[Header("Debug")]
        public float move;
        public void Btn_Slider()
        {
            move = !isActive ? 400 : 700;
            LeanTween.moveLocalX(gameObject, move, .7f).setEase(LeanTweenType.easeInOutCirc);
            isActive = !isActive;
        }

        
    }
}
