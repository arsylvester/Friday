using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TutorialState : MonoBehaviour
{
    [SerializeField] bool skipTutorial = false;

    private void Start()
    {
        if(skipTutorial)
        {
            state = "done";
        }
    }

    private static string[] STATES =
    {
        "tutorialStart",
        "mainMenu1",
        "leaveConvo",
        "inspectorTutorial",
        "inspectorTutorial0",
        "inspectorTutorial1",
        "inspectorTutorial2",
        "inspectorTutorial3",
        "inspectorTutorial4",
        "reenterConvo",
        "followUpQuestions",
        "recordDialogue",
        "mainMenu2",
        "interrogate",
        "comboAttempt1",
        "failCombo",
        "free",
        "deduction",
        "done"
    };

    private static string state = STATES[0];

    public static void Next()
    {
        int index = Array.FindIndex(STATES, x => x == state);
        if(index + 1 < STATES.Length)
            state = STATES[Array.FindIndex(STATES, x => x == state) + 1];
    }

    public static string Current
    {
        get { return state; }
    }
}
