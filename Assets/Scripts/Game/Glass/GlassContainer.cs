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
        [SerializeField] int cachedGlassCode = 0;

        public void Start()
        {
            updateSpawn();
        }

        public int getCode() => cachedGlassCode++;

        public GlassRegistered findEmptyGlass() => glassRegistereds.Find(res => res.glass.lastIgrendients == enumIgrendients.NULL);

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
