using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GlassContainer : Singleton<GlassContainer>
    {
        [Header("Properties")]
        public GameObject glassPrefab;
        public List<Transform> listPosSpawn;

        [Header("Debug")]
        public List<GlassRegistered> glassRegistereds = new List<GlassRegistered>();
        public bool canSpawn = false;
        [SerializeField] int cachedGlassCode = 0;

        public void Start()
        {
            updateSpawn();
        }

        private void LateUpdate()
        {
            if (canSpawn)
            {
                updateSpawn();
            }
        }

        public int getCode() => cachedGlassCode++;

        public void glassOnDestroy(GlassRegistered _glassRegistered)
        {
            //glassRegistereds
        }

        /// <summary>
        /// Find empty glass
        /// </summary>
        /// <returns></returns>
        public GlassRegistered findEmptyGlass() => glassRegistereds.Find(res => res.glass.lastIgrendients == enumIgrendients.NULL);

        /// <summary>
        /// Find glass with multiple state
        /// </summary>
        /// <param name="_enumIgrendients"></param>
        /// <returns></returns>
        public GlassRegistered findGlassWithState(List<enumIgrendients> _enumIgrendients) => glassRegistereds.Find(res => _enumIgrendients.Contains(res.glass.lastIgrendients));

        public void updateSpawn()
        {
            foreach (Transform _transform in listPosSpawn)
            {
                if(_transform.childCount == 0)
                {
                    Instantiate(glassPrefab, _transform);
                }
            }
        }
    }
}
