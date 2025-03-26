using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    //Object della UI
    public GameObject settingsMenu, mainMenu;

    [SerializeField] PlayerLobbyUI playerLobbyUI;

    public void Awake()
    {
        settingsMenu.SetActive(false);

        playerLobbyUI.SetEvent();

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
