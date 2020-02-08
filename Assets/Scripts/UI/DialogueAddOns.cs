using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueAddOns : MonoBehaviour
{
    private DialogueRunner dialogRunnner;
    private DialogueUI dialogUI;

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
    }

    public void LineIsPlaying()
    {
        linePlaying = true;
    }

    public void LineEnded()
    {
        linePlaying = false;
    }

}
