using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class UILevelStage : MonoBehaviour
{
    [Header("Properties")]
    public GameObject levelPrefab;

    [Header("Debug")]
    public List<UILevelChild> LevelGO;
    public bool isAcceptable = true;      // Prevent brute force req
    public int resourceLevelCount;

    private void OnEnable()
    {
        isAcceptable = true;
    }

    private void Start()
    {
        resourceLevelCount = ResourceManager.ListLevels().Count;
        int render = GlobalController.Instance.minLevelRender > resourceLevelCount ? GlobalController.Instance.minLevelRender : resourceLevelCount;
        for (int i = 0; i < render; i++)
        {
            instanceLevelChild(i);
        }
    }

    void instanceLevelChild(int _index)
    {
        UILevelChild lev = Instantiate(levelPrefab, transform).GetComponent<UILevelChild>();
        LevelGO.Add(lev);

        lev.init(
            this,
            _index,
            getLevelBase(_index)
            );
    }

    public LevelBase getLevelBase(int _index)
    {
        if (_index >= resourceLevelCount) return null;
        return ResourceManager.ListLevels()[_index];
    }

    public void reqFromChild(int _level)
    {
        isAcceptable = false;
        print($"req from child {_level}");
        GameManager.Instance.loadLevel(_level);
    }
}
