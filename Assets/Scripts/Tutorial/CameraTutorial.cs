using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTutorial : MonoBehaviour
{
    public GameObject inspectorPanel;
    public GameObject camPromptPanel;

    // Start is called before the first frame update
    void Start()
    {
        camPromptPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
            Debug.Log(camPromptPanel.activeInHierarchy && Input.GetMouseButtonDown(1));

        if (camPromptPanel.activeInHierarchy && Input.GetMouseButtonDown(1))
        {
            camPromptPanel.SetActive(false);
            inspectorPanel.SetActive(false);
            this.enabled = false;
        }
    }

    public void StartCamTut()
    {
        StartCoroutine(CamTut());
    }

    IEnumerator CamTut()
    {
        yield return new WaitForSeconds(2.6f);

        inspectorPanel.SetActive(true);
        camPromptPanel.SetActive(true);
    }
}
