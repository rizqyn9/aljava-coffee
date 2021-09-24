using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MenuType", menuName = "ScriptableObject/MenuType")]
public class MenuType : ScriptableObject
{
    public menuListName menuListName;
    public string menuName;
    public string menuDesc;
    public GameObject menuPrefab;
    public Sprite menuSprite;
    public List<enumIgrendients> recipe;
    public int price;
    public int pointInGame;
}
