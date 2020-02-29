using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;
using UnityEngine.Events;

public class DilogueTutorialManager : MonoBehaviour
{
    [SerializeField] Button convoAgainButton;
    [SerializeField] Button leaveButton;
    [SerializeField] Button followUpQButton;
    [SerializeField] Button interrogateButton;
    [SerializeField] Button stockingButton;
    [SerializeField] Button fridayAloneButton;
    [SerializeField] Button interrogateConfrimButton;
    [SerializeField] Button continueButton;
    
    [SerializeField] GameObject MM1Prompt;
    [SerializeField] GameObject leavePrompt;
    [SerializeField] GameObject FUQPrompt;
    [SerializeField] GameObject recDiaPrompt;
    [SerializeField] GameObject MM2Prompt;
    [SerializeField] GameObject interrogatePrompt;
    [SerializeField] GameObject comboAttemptPrompt;
    [SerializeField] GameObject failComboPrompt;
    [SerializeField] GameObject stockingPrompt;
    [SerializeField] TutorialPrompt tutorialPrompt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator waitThenPrompt(GameObject highlight, GameObject currrPrompt, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        tutorialPrompt.StartPrompt(highlight, currrPrompt, true);
    }

    [YarnCommand("tutorialprompt")]
    public void prompt()
    {
        print("Running prompt with state: " + TutorialState.Current);
        switch(TutorialState.Current)
        {
            case "tutorialStart":
                TutorialState.Next();
                break;
            case "mainMenu1":
                //convoAgainButton.gameObject.SetActive(true);
                StartCoroutine(waitThenPrompt(convoAgainButton.gameObject, MM1Prompt, .05f));
                break;
            case "leaveConvo":
                leaveButton.gameObject.SetActive(true);
                StartCoroutine(waitThenPrompt(leaveButton.gameObject, leavePrompt, .9f));
                break;
            case "followUpQuestions":
                StartCoroutine(waitThenPrompt(followUpQButton.gameObject, FUQPrompt, .05f));
                break;
            case "recordDialogue":
                break;
            case "mainMenu2":
                break;
            case "interrogate":
                break;
            case "comboAttempt1":
                break;
            case "failCombo":
                break;
            case "stocking":
                break;
            case "done":
                break;
            default:
                break;
        }
    }

    public void EndTutorial()
    {
        print("Ending Tutorial");
        switch(TutorialState.Current)
        {
            case "mainMenu1":
                convoAgainButton.onClick.SetPersistentListenerState(2, UnityEventCallState.Off);
                break;
             case "leaveConvo":
                leaveButton.onClick.SetPersistentListenerState(4, UnityEventCallState.Off);
                break;
            case "followUpQuestions":
                followUpQButton.onClick.SetPersistentListenerState(2, UnityEventCallState.Off);
                break;
            case "recordDialogue":
                break;
            case "mainMenu2":
                break;
            case "interrogate":
                break;
            case "comboAttempt1":
                break;
            case "failCombo":
                break;
            case "stocking":
                break;
            case "done":
                break;
            default:
                break;
        }
        tutorialPrompt.EndPrompt();
    }

    //For Testing
    [YarnCommand("nextTutState")]
    public void nextTutState()
    {
        TutorialState.Next();
        TutorialState.Next();
        print(TutorialState.Current);
    }
}
