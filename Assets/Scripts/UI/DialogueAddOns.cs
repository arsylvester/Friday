using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;

public class DialogueAddOns : MonoBehaviour
{
    private DialogueRunner dialogRunnner;
    private DialogueUI dialogUI;
    [SerializeField] Journal journal;

    GameObject fadeImage;

    private bool linePlaying = false;

    private void Start()
    {
        dialogRunnner = GetComponent<DialogueRunner>();
        dialogUI = GetComponent<DialogueUI>();

        fadeImage = GameObject.FindGameObjectWithTag("Fade");
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && linePlaying)
        {
            dialogUI.MarkLineComplete();
        }

        if(fadeImage == null)
        {
            fadeImage = GameObject.FindGameObjectWithTag("Fade");
        }

        if (Input.GetKeyDown(KeyCode.Escape) && dialogRunnner.isDialogueRunning && (TutorialState.Current == "deduction" || TutorialState.Current == "done") && !fadeImage.GetComponent<Image>().enabled)
        {
            if (dialogRunnner.currentNodeName == "AbbyInterogation")
            {
                dialogUI.SelectOption(1);
            }
            else
            {
                Leave();
            }
        }
    }

    public void LineIsPlaying()
    {
        linePlaying = true;
    }

    public void LineEnded()
    {
        linePlaying = false;
    }

    public void Leave()
    {
        dialogRunnner.Stop();
        dialogUI.DialogueComplete();
        journal.CloseJournals();
    }

}
