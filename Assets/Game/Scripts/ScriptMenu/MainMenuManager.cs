using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    //Object della UI
    public GameObject settingsMenu, mainMenu;

    public void Awake()
    {
        settingsMenu.SetActive(false);
    }
    public void ActivateSettingsMenu()
    {
        settingsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void DeactivateSettingMenu()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
