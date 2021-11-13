using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject HowToPlay;
    public void GoToGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void GoToInstructions()
    {
        SceneManager.LoadScene("Instructions");
    }

    public void ShowHowToPlay()
    {
        MainMenu.SetActive(false);
        HowToPlay.SetActive(true);
    }

    public void HideHowToPlay()
    {
        HowToPlay.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
