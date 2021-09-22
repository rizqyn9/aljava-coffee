using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public struct GlassRegistered
    {
        public string glassCode;
        public Glass glass;
    }

    public class GlassContainer : Singleton<GlassContainer>
    {
        [Header("Properties")]
        public GameObject glassPrefab;
        public List<Transform> listPosSpawn;
        int delay = 2;

        [Header("Debug")]
        public List<GlassRegistered> glassRegistereds = new List<GlassRegistered>();
        [SerializeField] int cachedGlassCode = 0;

        public void Start()
        {
            updateSpawn();
        }

        public void respawn()
        {
            StartCoroutine(spawnGlass());
        }

        public int getCode() => cachedGlassCode++;

        public void glassOnDestroy(GlassRegistered _glassRegistered)
        {
            //glassRegistereds
        }

        /// <summary>
        /// Find last igrendients from param,
        /// Find Glass State not equals Glass.OnProcess
        /// </summary>
        /// <returns></returns>
        public GlassRegistered findGlassLastState(enumIgrendients _lastIgrendient) => glassRegistereds.Find(res => res.glass.lastIgrendients == _lastIgrendient && res.glass.glassState != GlassState.PROCESS);

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

        IEnumerator spawnGlass()
        {
            yield return new WaitForSeconds(delay);
            updateSpawn();
        }
    }
}
