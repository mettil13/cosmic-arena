using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    //Object della UI
    public GameObject settingsMenu, mainMenu;
    public AudioClip buttonSFX;

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

    public void CreditsScene()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void PlayButtonSFXSound()
    {
        AudioManager.Instance.PlaySFX(buttonSFX);
    }
}
