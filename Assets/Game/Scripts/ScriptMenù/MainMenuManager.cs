using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    //Object della UI
    public GameObject settingsMenu;

    public void Awake()
    {
        settingsMenu.SetActive(false);
    }
    public void ActivateSettingsMenu()
    {
        settingsMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
