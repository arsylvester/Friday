using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;
using System;

public class DialogueAddOns : MonoBehaviour
{
    private DialogueRunner dialogRunnner;
    private DialogueUI dialogUI;
    [SerializeField] Journal journal;
    [SerializeField] Text NameTextbox;

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
            if (dialogRunnner.currentNodeName == "AbbyInterogation" || dialogRunnner.currentNodeName == "BernardInterrogation" || dialogRunnner.currentNodeName == "LeonardInterogation" || dialogRunnner.currentNodeName == "TristanInterogation" || dialogRunnner.currentNodeName == "ComputerMain" || dialogRunnner.currentNodeName == "ComputerOpen" || dialogRunnner.currentNodeName == "SafeMain")
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

    public void SetSpeakerName()
    {
        string currLine = dialogUI.currentText;
        print("currLine: " + currLine);
        if (currLine != "")
        {
            string[] separator = { ":" };
            int count = 2;

            // using the method 
            string[] strlist = currLine.Split(separator, count,
                   StringSplitOptions.RemoveEmptyEntries);
            print(strlist);
            NameTextbox.text = strlist[0];
        }
    }

    public void ClearSpeakerName()
    {
        NameTextbox.text = "";
    }
}
