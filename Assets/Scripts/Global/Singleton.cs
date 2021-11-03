using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    /// <summary>
    /// Temporary turn off
    /// </summary>
    //public abstract bool isDestroyed();

    [Header("Singleton Properties")]
    public bool isDDOL = false;

    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject GO = new GameObject();
                    _instance = GO.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this as T;
        if (isDDOL)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
