using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void GoToGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void GoToInstructions()
    {
        SceneManager.LoadScene("Instructions");
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
