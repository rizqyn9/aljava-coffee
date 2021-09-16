using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    /// <summary>
    /// Temporary turn off
    /// </summary>
    //public abstract bool isDestroyed();

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

    protected virtual void Awake()
    {
        _instance = this as T;
    }
}
