﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void GoToGame()
    {
        SceneManager.LoadScene("Vinson_Movement_Cam_1");
    }

    public void GoToSettings()
    {
        SceneManager.LoadScene("Settings");
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
