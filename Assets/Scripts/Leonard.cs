using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;

public class Leonard : MonoBehaviour
{
    public GameObject mapUI;
    public float fadeIn;
    PlayerMovement playerMove;
    DialogueCam cam;
    Journal journal;
    LevelController levelControl;

    [SerializeField] Image fadePanel;
    [SerializeField] GameObject mapBackButton;

    private void Start()
    {
        playerMove = FindObjectOfType<PlayerMovement>();
        cam = FindObjectOfType<DialogueCam>();
        journal = FindObjectOfType<Journal>();
        levelControl = FindObjectOfType<LevelController>();
    }

    //When Leonard's room is finished
    IEnumerator OpenMapUIEnd()
    {
        yield return new WaitForSeconds(5);
        playerMove.enabled = false;
        playerMove.StopMovement();
        

        yield return new WaitForSeconds(5);
        journal.CloseJournals();
        journal.CanOpenJournals(false);
        fadePanel.enabled = true;
        fadePanel.canvasRenderer.SetAlpha(0f);
        // fade in
        fadePanel.CrossFadeAlpha(1, fadeIn, false);
        yield return new WaitForSeconds(fadeIn);

        mapUI.SetActive(true);

       // cam.FreezeCamera();


        mapBackButton.SetActive(false);
        levelControl.cancloseMap = false;
    }

    //When kicked back to map for not having a warrent
    IEnumerator OpenMapUIStart()
    {
        journal.CloseJournals();
        journal.CanOpenJournals(false);
        mapBackButton.SetActive(false);
        levelControl.cancloseMap = false;

        mapUI.SetActive(true);
        yield return new WaitForSeconds(5);
        playerMove.enabled = false;
        playerMove.StopMovement();  
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
        //cam.StartFade(true);
        StartCoroutine(OpenMapUIEnd()); 
    }

    [YarnCommand("NoWarrent")]
    public void NoWarrent()
    {
        StartCoroutine(OpenMapUIStart());
    }
}
