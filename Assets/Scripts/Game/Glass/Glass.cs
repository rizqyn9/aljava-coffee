using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Glass : MonoBehaviour
    {
        [Header("Properties")]
        public string glassCode;

        private void Start()
        {
            glassCode = generateUniqueCode();
            gameObject.name = glassCode;
            LevelManager.Instance.listGlassRegistered.Add(new GlassRegistered() { glassCode = glassCode, glass = this });
        }

        private string generateUniqueCode() => $"--Glass{GlassContainer.Instance.getCode()}";
    }
}
