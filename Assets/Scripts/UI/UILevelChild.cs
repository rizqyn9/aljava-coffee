using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UILevelChild : MonoBehaviour
{
    [Header("Properties")]
    public Image baseSprite;
    public Color32 baseLocked;
    public TMP_Text text;
    public Button btn;

    [Header("Debug")]
    public bool isOpened = false;
    public LevelBase levelBase;
    [SerializeField] int level;
    [SerializeField] UILevelStage uILevelStage;

    public void init(UILevelStage _uILevelStage, int _index, LevelBase _levelBase = null)
    {
        uILevelStage = _uILevelStage;

        level = _index + 1;
        gameObject.name = $"Level-{level}";
        text.text = (level).ToString();

        if (_levelBase)
        {
            levelBase = _levelBase;
        } else
        {
            baseSprite.color = baseLocked;
            text.color = new Color(0, 1, 0, .5f);
            btn.interactable = false;
            Debug.LogWarning("Somenthing level have null");
            return;
        }

        if (!checkIsOpen(_levelBase.level))
        {
            baseSprite.color = baseLocked;
            text.color = new Color(0, 0, 0, .8f);
            btn.interactable = false;
            return;
        }

        btn.onClick.AddListener(ClickHandle);       // Add Listener
    }

    bool checkIsOpen(int _level)
    {
        return GameManager.Instance.saveData.userData.listLevels.Find(val => val.level == _level).isOpen;
    }

    private void ClickHandle()
    {
        uILevelStage = gameObject.GetComponentInParent<UILevelStage>();
        if (uILevelStage.isAcceptable)              // Prevent brute force req
        {
            uILevelStage.reqFromChild(level);
        }
    }
}
