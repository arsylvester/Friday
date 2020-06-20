using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleActions : MonoBehaviour
{
    public void LoadScene(string scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
