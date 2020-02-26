using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTutorial : MonoBehaviour
{
    [SerializeField] GameObject testPrompts;
    [SerializeField] TutorialPrompt tutorialPrompt;

    public void StartTest()
    {
        tutorialPrompt.StartPrompt(gameObject, testPrompts);
        GetComponent<Button>().onClick.AddListener(EndTest);
        GetComponent<Button>().onClick.RemoveListener(StartTest);
    }

    public void EndTest()
    {
        tutorialPrompt.EndPrompt();
    }
}
