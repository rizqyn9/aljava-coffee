using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
    public GameObject SettingsBox;
    public GameObject MenuListBox;
    public GameObject ExitBox;
    public GameObject AboutBox;
    public GameObject OrderBox;
    // Start is called before the first frame update
    void Start()
    {
        SettingsBox.SetActive(false);
        MenuListBox.SetActive(false);
        ExitBox.SetActive(false);
        AboutBox.SetActive(false);
        OrderBox.SetActive(false);
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
    public void OpenAbout()
    {
        AboutBox.SetActive(true);
    }
    public void CloseAbout()
    {
        AboutBox.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void OpenBuy()
    {
        OrderBox.SetActive(true);
    }
    public void CloseBuy()
    {
        OrderBox.SetActive(false);
    }
    public void OpenMaps()
    {
        Application.OpenURL("https://www.google.com/maps/place/Aljava+Cafe/@-6.8079354,110.8404192,15z/data=!4m5!3m4!1s0x0:0x695b061a6027cab2!8m2!3d-6.8080939!4d110.8403623");
    }
    public void OpenSocials()
    {
        Application.OpenURL("https://www.instagram.com/aljavacafe/");
    }
    public void OpenGrabFood()
    {
        Application.OpenURL("https://food.grab.com/id/en/restaurant/aljava-cafe-demaan-delivery/6-C2E1RFKATUKJJN");
    }
}
