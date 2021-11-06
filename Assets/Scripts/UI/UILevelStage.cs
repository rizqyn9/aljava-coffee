using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILevelStage : MonoBehaviour
{
    public GameObject levelPrefab;
    public List<GameObject> LevelGO;

    private void Start()
    {
        for(int i = 0; i < 20; i++)
        {
            instanceLevelChild(i);
        }
    }

    void instanceLevelChild(int _index)
    {
        GameObject go = Instantiate(levelPrefab, transform);
        go.name = $"Level-{_index}";
    }
}
