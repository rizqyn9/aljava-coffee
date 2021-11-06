using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class UILevelChild : MonoBehaviour
{
    [Header("Properties")]
    public Image baseSprite;
    public TMP_Text text;
    public Button btn;

    [Header("Debug")]
    public bool isOpened = false;
    [SerializeField] int level;
    [SerializeField] UILevelStage uILevelStage;

    public void init(int _index)
    {
        level = _index + 1;
        gameObject.name = $"Level-{level}";
        text.text = (level).ToString();

        btn.onClick.AddListener(ClickHandle);       // Add Listener
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
