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
        [SerializeField] int cachedGlassCode = 0;

        public void Start()
        {
            updateSpawn();
        }

        public int getCode() => cachedGlassCode++;

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
