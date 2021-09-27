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
        public int seatIndex;
    }

    public class GlassContainer : Singleton<GlassContainer>
    {
        [Header("Properties")]
        public GameObject glassPrefab;
        public List<Transform> listPosSpawn;
        public float delay = .2f;

        [Header("Debug")]
        [SerializeField] List<GlassRegistered> glassRegistereds = new List<GlassRegistered>();
        [SerializeField] int cachedGlassCode = 0;

        public void Start()
        {
            for(int i = 0; i < listPosSpawn.Count; i++)
            {
                reqGlassSpawn(i);
            }
        }

        public void glassOnDestroy(GlassRegistered _glassRegistered)
        {
            //glassRegistereds
        }

        public void reqGlassSpawn(int _seat)
        {
            Glass _spawn = Instantiate(glassPrefab, listPosSpawn[_seat]).GetComponent<Glass>();

            GlassRegistered _registGlass = new GlassRegistered()
            {
                glass = _spawn,
                glassCode = generateUniqueCode(),
                seatIndex = _seat
            };
            StartCoroutine(spawnGlass(_spawn.gameObject));
            glassRegistereds.Add(_registGlass);
            _spawn.glassRegistered = _registGlass;
        }

        IEnumerator spawnGlass(GameObject _glass)
        {
            _glass.transform.localScale = Vector2.zero;
            _glass.LeanScale(new Vector2(2f, 2f), delay).setEaseInBounce();
            yield break;

        }

        #region Depends

        public string generateUniqueCode() => $"Glass-{cachedGlassCode++}";

        /// <summary>
        /// Find last igrendients from param,
        /// Find Glass State not equals Glass.OnProcess
        /// </summary>
        /// <returns></returns>
        public static GlassRegistered FindGlassLastState(MachineIgrendient _lastIgrendient)
        {
            return Instance.glassRegistereds.Find(res => res.glass.lastIgrendients == _lastIgrendient && res.glass.glassState != GlassState.PROCESS);
        }

        /// <summary>
        /// Find glass with multiple state
        /// </summary>
        /// <param name="_MachineIgrendient"></param>
        /// <returns></returns>
        public GlassRegistered findGlassWithState(List<MachineIgrendient> _MachineIgrendient)
        {
            return glassRegistereds.Find(res => _MachineIgrendient.Contains(res.glass.lastIgrendients));
        }

        public static bool IsGlassTargetAvaible(MachineIgrendient _igrend,out GlassRegistered _glassRegistered)
        {
            _glassRegistered = FindGlassLastState(_igrend);
            return _glassRegistered.glassCode != null;
        }

        #endregion
    }
}
