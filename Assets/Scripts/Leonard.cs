using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;

public class Leonard : MonoBehaviour
{
    public GameObject mapUI;
    PlayerMovement playerMove;
    InputFreeLookCam cam;
    Journal journal;

    [SerializeField] Image fadePanel;
    [SerializeField] GameObject mapBackButton;

    private void Start()
    {
        playerMove = FindObjectOfType<PlayerMovement>();
        cam = FindObjectOfType<InputFreeLookCam>();
        journal = FindObjectOfType<Journal>();
    }

    IEnumerator OpenMapUI()
    {
        yield return new WaitForSeconds(5);
        fadePanel.enabled = true;
        mapUI.SetActive(true);

        playerMove.StopMovement();

        cam.FreezeCamera();

        journal.CloseJournals();
        journal.CanOpenJournals(false);

        mapBackButton.SetActive(false);
    }

    IEnumerator FadeToBlack()
    {
        fadePanel.enabled = true;
        fadePanel.color = new Color(0, 0, 0, 0);
        for (int x = 0; x < 100; x++)
        {
            fadePanel.color = fadePanel.color + new Color(0, 0, 0, .01f);
            yield return new WaitForEndOfFrame();
        }
    }

    [YarnCommand("LeonardEnd")]
    public void LeonardEnd()
    {
        print("Leonard End started");
        //StartCoroutine(FadeToBlack());
        StartCoroutine(OpenMapUI()); 
    }
}
