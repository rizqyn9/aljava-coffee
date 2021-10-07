using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentClass<T1>: Singleton<T1> where T1 : Component
{
    public GameObject okay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
