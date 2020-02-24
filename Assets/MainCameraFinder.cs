using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCameraFinder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += ReloadCamera;
        ReloadCamera(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    public void ReloadCamera(Scene scene, LoadSceneMode mode)
    {
        GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
}
