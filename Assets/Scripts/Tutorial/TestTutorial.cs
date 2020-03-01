using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTutorial : MonoBehaviour
{
    [SerializeField] GameObject testPrompts;
    [SerializeField] TutorialPrompt tutorialPrompt;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(StartTest);
    }

    public void StartTest()
    {
        tutorialPrompt.StartPrompt(gameObject, testPrompts, true);
        GetComponent<Button>().onClick.AddListener(EndTest);
        GetComponent<Button>().onClick.RemoveListener(StartTest);
    }

    public void EndTest()
    {
        tutorialPrompt.EndPrompt();
    }
}
