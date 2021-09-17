using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Bubble : MonoBehaviour
    {
        [Header("Properties")]
        public List<Transform> spawnMenuPos = new List<Transform>();
        public GameObject basePrefabMenuItem;

        [Header("Debug")]
        public List<GameObject> menuSpawnedGO;
        public Dictionary<int, MenuType> menuList = new Dictionary<int, MenuType>();
        public int menuIndex = 0;

        public void setMenu(List<MenuType> _menuTypes)
        {
            foreach(MenuType _menuType in _menuTypes)
            {
                menuList.Add(menuIndex++, _menuType);
            }
        }

        public void render()
        {
            foreach(KeyValuePair<int,MenuType> _menuItem in menuList)
            {
                GameObject GO = Instantiate(_menuItem.Value.menuPrefab, spawnMenuPos[_menuItem.Key]);
                menuSpawnedGO.Add(GO);
            }
        }
    }
}
