using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public GameObject SettingsBox;
    public GameObject MenuListBox;
    public GameObject ExitBox;
    // Start is called before the first frame update
    void Start()
    {
        SettingsBox.SetActive(false);
        MenuListBox.SetActive(false);
        ExitBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void OpenSettings()
    {
        SettingsBox.SetActive(true);
    }
    public void CloseSettings()
    {
        SettingsBox.SetActive(false);
    }

    public void OpenMenuList()
    {
        MenuListBox.SetActive(true);
    }
    public void CloseMenuList()
    {
        MenuListBox.SetActive(false);
    }
    public void OpenExit()
    {
        ExitBox.SetActive(true);
    }
    public void CloseExit()
    {
        ExitBox.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
