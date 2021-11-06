using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILevelStage : MonoBehaviour
{
    [Header("Properties")]
    public GameObject levelPrefab;

    [Header("Debug")]
    public List<UILevelChild> LevelGO;
    public bool isAcceptable = true;      // Prevent brute force req

    private void Start()
    {
        for(int i = 0; i < 20; i++)
        {
            instanceLevelChild(i);
        }
    }

    void instanceLevelChild(int _index)
    {
        UILevelChild lev = Instantiate(levelPrefab, transform).GetComponent<UILevelChild>();
        LevelGO.Add(lev);
        lev.init(_index);
    }

    public void reqFromChild(int _index)
    {
        isAcceptable = false;
        print($"req from child{_index}");
    }
}
