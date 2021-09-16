using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Glass : MonoBehaviour
    {
        [Header("Properties")]
        public string glassCode;
        public Transform igrendientTransform;
        public List<enumIgrendients> igrendients;
        public enumIgrendients lastIgrendients = enumIgrendients.NULL;

        [Header("Debug")]
        public List<GameObject> listIgrendientsGO = new List<GameObject>();

        private void Start()
        {
            glassCode = generateUniqueCode();
            gameObject.name = glassCode;
            GlassRegistered glassRegistered = new GlassRegistered() { glassCode = glassCode, glass = this };
            LevelManager.Instance.listGlassRegistered.Add(glassRegistered);
            GlassContainer.Instance.glassRegistereds.Add(glassRegistered);
        }

        private string generateUniqueCode() => $"--Glass{GlassContainer.Instance.getCode()}";

        public void addIgredients(GameObject _prefab, enumIgrendients _igrendient)
        {
            GameObject GO = Instantiate(_prefab, igrendientTransform);
            listIgrendientsGO.Add(GO);
            lastIgrendients = _igrendient;
            igrendients.Add(_igrendient);
        }
    }
}
