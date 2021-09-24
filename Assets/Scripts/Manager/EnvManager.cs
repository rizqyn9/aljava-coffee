using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public interface IEnv
    {
        void EnvInstance();
    }

    public class EnvManager : Singleton<EnvManager>
    {
        [Header("Properties")]
        public List<Transform> transforms = new List<Transform>();
        public List<GameObject> prefabList = new List<GameObject>();
        public GlassContainer glassContainer;

        [Header("Debug")]
        [SerializeField] List<GameObject> resGO = new List<GameObject>();
        [SerializeField] List<IEnv> envs = new List<IEnv>();


        public void Spawn()
        {
            for (int i = 0; i < transforms.Count; i++)
            {
                GameObject go = Instantiate(prefabList[i], transforms[i]);
                go.LeanAlpha(0, 0);
                go.LeanMoveLocalY(1, 0);
                IEnv env = GetOutType<IEnv>(go);
                if (env != null)
                {
                    envs.Add(env);
                }
            }
        }

        public void Init()
        {
            Spawn();
            print(envs.Count);
            foreach(IEnv _env in envs)
            {
                _env.EnvInstance();
            }
        }

        public static T GetOutType<T>(GameObject _go)
        {
            T _out = _go.GetComponent<T>();
            return _out;
        }
    }
}
