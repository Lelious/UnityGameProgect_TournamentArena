using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControllerScript : MonoBehaviour
{
    public GameObject buttonsMainMenu;
    public GameObject buttonsSettingsMenu;
    public GameObject buttonsExitMenu;
    public GameObject buttonsPlayerMenu;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void ShowExitMenu()
    {
        buttonsMainMenu.SetActive(false);
        buttonsExitMenu.SetActive(true);        
    }
    public void ShowMainMenuOnExit()
    {
        buttonsExitMenu.SetActive(false);
        buttonsMainMenu.SetActive(true);
    }
    public void ShowSettingsMenu()
    {        
        buttonsMainMenu.SetActive(true);
        buttonsSettingsMenu.SetActive(true);
    }
    public void ShowMainMenuOnSettings()
    {
        buttonsSettingsMenu.SetActive(false);
        buttonsMainMenu.SetActive(true);
    }
    public void ShowPlayerMenu()
    {
       buttonsMainMenu.SetActive(false);
       buttonsPlayerMenu.SetActive(true);
    }
    public void ShowMainMenuOnPlayerMenu()
    {
        buttonsPlayerMenu.SetActive(false);
        buttonsMainMenu.SetActive(true);
    }
    public void LoadSinglePlayerGame()
    {
        SceneManager.LoadScene("SinglePlayerGame");
    }
    public void QuitApp()
    {
        Application.Quit();
    }
}
