using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraTutorial : MonoBehaviour
{
    [SerializeField] GameObject inspectorPanel;
    [SerializeField] GameObject camPromptPanel;

    bool isTutDone;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += ReloadCameraTutorial;
        ReloadCameraTutorial(SceneManager.GetActiveScene(), LoadSceneMode.Single);

        camPromptPanel.SetActive(false);
        isTutDone = false;
    }

    public void ReloadCameraTutorial(Scene scene, LoadSceneMode mode)
    {
        inspectorPanel = FindObjectOfType<TutorialPrompt>().gameObject;
        camPromptPanel = inspectorPanel.transform.GetChild(5).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (camPromptPanel && camPromptPanel.activeInHierarchy && Input.GetMouseButtonDown(1))
        {
            camPromptPanel.SetActive(false);
            inspectorPanel.SetActive(false);
            this.enabled = false;
        }
    }

    public void StartCamTut()
    {
        if (!isTutDone)
        {
            StartCoroutine(CamTut());
            isTutDone = true;
        }
    }

    IEnumerator CamTut()
    {
        yield return new WaitForSeconds(2.6f);

        inspectorPanel.SetActive(true);
        camPromptPanel.SetActive(true);
    }
}
