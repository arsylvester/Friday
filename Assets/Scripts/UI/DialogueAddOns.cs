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

    private bool linePlaying = false;

    private void Start()
    {
        dialogRunnner = GetComponent<DialogueRunner>();
        dialogUI = GetComponent<DialogueUI>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && linePlaying)
        {
            dialogUI.MarkLineComplete();
        }

        print(dialogRunnner.isDialogueRunning);
        if (Input.GetKeyDown(KeyCode.Escape) && dialogRunnner.isDialogueRunning && (TutorialState.Current == "deduction" || TutorialState.Current == "done"))
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
