using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TutorialState : MonoBehaviour
{
    private static string[] STATES =
    {
        "tutorialStart",
        "mainMenu1",
        "leaveConvo",
        "inspectorTutorial",
        "reenterConvo",
        "followUpQuestions",
        "recordDialogue",
        "mainMenu2",
        "interrogate",
        "comboAttempt1",
        "failCombo",
        "stocking",
        "done"
    };

    private static string state = STATES[0];

    public static void Next()
    {
        state = STATES[STATES.ToList().FindIndex(x => x == state) + 1];
    }

    public static string Current
    {
        get { return state; }
    }
}
