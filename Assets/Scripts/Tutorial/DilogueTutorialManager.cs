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
    [SerializeField] GameObject recDiaHighlight;
    [SerializeField] Button interrogateButton;
    [SerializeField] GameObject interHighlight;
    public Button stockingButton;
    public Button fridayAloneButton;
    [SerializeField] Button interrogateConfrimButton;
    [SerializeField] Button interrogateQuitButton;
    [SerializeField] Button continueButton;
    [SerializeField] Button interogateButton1;
    [SerializeField] Button interogateButton2;
    [SerializeField] Button interogateButton3;
    [SerializeField] Button interogateButton4;
    public Button deductionButton;

    public Button startCamTutButton;
    CameraTutorial camTutorial;
    
    [SerializeField] GameObject MM1Prompt;
    [SerializeField] GameObject leavePrompt;
    [SerializeField] GameObject FUQPrompt;
    [SerializeField] GameObject recDiaPrompt;
    [SerializeField] GameObject MM2Prompt;
    [SerializeField] GameObject interrogatePrompt1;
    [SerializeField] GameObject interrogatePrompt2;
    [SerializeField] GameObject interrogatePrompt3;
    [SerializeField] GameObject interrogatePrompt4;
    [SerializeField] GameObject comboAttemptPrompt;
    //[SerializeField] GameObject failComboPrompt;
    [SerializeField] GameObject stockingPrompt;
    [SerializeField] GameObject freePrompt;
    [SerializeField] GameObject deduectionPrompt;
    [SerializeField] TutorialPrompt tutorialPrompt;
    [SerializeField] GameObject arrow;

    private bool stockingPressed = false;
    private bool fridayAlonePressed = false;
    private bool deductionPressed = false;
    private bool continuePressed = false;

    public bool canSaveDialogue = false;
    //testing
    private bool skipStates = true;
    public bool turnTutorialOff = false;

    private void Start()
    {
        if (turnTutorialOff == true)
            canSaveDialogue = true;

        camTutorial = FindObjectOfType<CameraTutorial>();
    }

    IEnumerator interrogatePrompts()
    {
        tutorialPrompt.StartPrompt(interHighlight, interrogatePrompt1, true, null);
        continuePressed = false;

        while (!continuePressed)
        {
             yield return null;
        }

        continuePressed = false;
        interrogatePrompt1.SetActive(false);
        interrogatePrompt2.SetActive(true);

        yield return new WaitForEndOfFrame();

        while(!continuePressed)
        {
             yield return null;
        }

        continuePressed = false;
        interrogatePrompt2.SetActive(false);
        interrogatePrompt3.SetActive(true);

        yield return new WaitForEndOfFrame();

        while(!continuePressed)
        {
             yield return null;
        }

        continuePressed = false;
        interrogatePrompt3.SetActive(false);
        interrogatePrompt4.SetActive(true);

        yield return new WaitForEndOfFrame();

        continuePressed = false;
        tutorialPrompt.StartPrompt(continueButton.gameObject, interrogatePrompt4, true, null);
        continueButton.onClick.AddListener(continueClicked);

        while(!continuePressed)
        {
             yield return null;
        }

        interrogatePrompt4.SetActive(false);
        continueButton.onClick.RemoveListener(continueClicked);

        EndTutorial();
        //prompt();
    }

    public void InterogateButtonPressed()
    {
        continuePressed = true;
    }

    public void fridayClicked()
    {
        fridayAlonePressed = !fridayAlonePressed;
    }

    public void stockingClicked()
    {
        stockingPressed = !stockingPressed;
        print("Stocking Clicked");
    }

    public void deductionClicked()
    {
        deductionPressed = !deductionPressed;
    }

    public void continueClicked()
    {
        continuePressed = !continuePressed;
    }

    IEnumerator comboAttempt()
    {
        tutorialPrompt.StartPrompt(fridayAloneButton.gameObject, comboAttemptPrompt, false, null);
        tutorialPrompt.StartPrompt(stockingButton.gameObject, comboAttemptPrompt, false, null);

        fridayAloneButton.onClick.AddListener(fridayClicked);
        stockingButton.onClick.AddListener(stockingClicked);

        fridayAlonePressed = false;
        stockingPressed = false;

        while(!fridayAlonePressed || !stockingPressed)
        {
            yield return new WaitForEndOfFrame();
        }

        GameObject highlightDup = Instantiate(interrogateConfrimButton.gameObject, interrogateConfrimButton.gameObject.transform.position, interrogateConfrimButton.gameObject.transform.rotation, interrogateConfrimButton.gameObject.transform.parent);
        highlightDup.transform.SetSiblingIndex(interrogateConfrimButton.gameObject.transform.GetSiblingIndex());
        tutorialPrompt.StartPrompt(interrogateConfrimButton.gameObject, comboAttemptPrompt, true, highlightDup);
        interrogateConfrimButton.onClick.SetPersistentListenerState(2, UnityEventCallState.RuntimeOnly);
    }

    IEnumerator stockingAttempt()
    {
        yield return new WaitForSecondsRealtime(.1f);
        tutorialPrompt.StartPrompt(stockingButton.gameObject, stockingPrompt, false, null);

        //stockingButton.onClick.AddListener(stockingClicked);

        stockingPressed = false;

        while(!stockingPressed)
        {
            yield return new WaitForEndOfFrame();
        }
        GameObject highlightDup = Instantiate(interrogateConfrimButton.gameObject, interrogateConfrimButton.gameObject.transform.position, interrogateConfrimButton.gameObject.transform.rotation, interrogateConfrimButton.gameObject.transform.parent);
        highlightDup.transform.SetSiblingIndex(interrogateConfrimButton.gameObject.transform.GetSiblingIndex());
        tutorialPrompt.StartPrompt(interrogateConfrimButton.gameObject, stockingPrompt, true, highlightDup);
        interrogateConfrimButton.onClick.SetPersistentListenerState(2, UnityEventCallState.RuntimeOnly);
    }

    public void startDeductionStep()
    {
        StartCoroutine(deductionStep());
    }
    
    IEnumerator deductionStep()
    {
        yield return new WaitForSeconds(.1f);
        tutorialPrompt.StartPrompt(deductionButton.gameObject, deduectionPrompt, true, null);
        deductionButton.onClick.AddListener(deductionClicked);
        deductionPressed = false;

        while(!deductionPressed)
        {
            yield return new WaitForEndOfFrame();
        }

        deductionButton.onClick.RemoveListener(deductionClicked);
        EndTutorial();
    }

    IEnumerator waitThenPrompt(GameObject highlight, GameObject currrPrompt, float waitTime)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        GameObject highlightDup = Instantiate(highlight, highlight.transform.position, highlight.transform.rotation, highlight.transform.parent);
        highlightDup.transform.SetSiblingIndex(highlight.transform.GetSiblingIndex());
        tutorialPrompt.StartPrompt(highlight, currrPrompt, true, highlightDup);
    }

    [YarnCommand("tutorialprompt")]
    public void prompt()
    {
        if (turnTutorialOff)
            return;
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
                continueButton.onClick.SetPersistentListenerState(0, UnityEventCallState.Off);
                leaveButton.gameObject.SetActive(true);
                StartCoroutine(waitThenPrompt(leaveButton.gameObject, leavePrompt, 1.5f));
                break;
            case "followUpQuestions":
                StartCoroutine(waitThenPrompt(followUpQButton.gameObject, FUQPrompt, .05f));
                break;
            case "recordDialogue":
                continueButton.onClick.SetPersistentListenerState(0, UnityEventCallState.Off);
                tutorialPrompt.StartPrompt(recDiaHighlight, recDiaPrompt, true, null);
                canSaveDialogue = true;
                break;
            case "mainMenu2":
                StartCoroutine(waitThenPrompt(interrogateButton.gameObject, MM2Prompt, .05f));
                break;
            case "interrogate":
                StartCoroutine(interrogatePrompts());
                break;
            case "comboAttempt1":
                StartCoroutine(comboAttempt());
                break;
            case "failCombo":
                StartCoroutine(stockingAttempt());
                break;
            case "free":
                continueButton.onClick.SetPersistentListenerState(2, UnityEventCallState.RuntimeOnly);
                StartCoroutine(waitThenPrompt(recDiaHighlight, freePrompt, .05f));
                break;
            case "deduction":
                break;
            case "done":
                break;
            default:
                break;
        }
    }

    public void EndTutorial()
    {
        if (turnTutorialOff)
            return;
        print("Ending Tutorial");
        
        switch(TutorialState.Current)
        {
            case "mainMenu1":
                convoAgainButton.onClick.SetPersistentListenerState(2, UnityEventCallState.Off);
                break;
             case "leaveConvo":
                leaveButton.onClick.SetPersistentListenerState(4, UnityEventCallState.Off);
                leaveButton.gameObject.SetActive(false);
                continueButton.onClick.SetPersistentListenerState(0, UnityEventCallState.RuntimeOnly);
                break;
            case "followUpQuestions":
                followUpQButton.onClick.SetPersistentListenerState(2, UnityEventCallState.Off);
                break;
            case "recordDialogue":
                continueButton.onClick.SetPersistentListenerState(0, UnityEventCallState.RuntimeOnly);
                break;
            case "mainMenu2":
                interrogateButton.onClick.SetPersistentListenerState(2, UnityEventCallState.Off);
                break;
            case "interrogate":
                break;
            case "comboAttempt1":
                interrogateConfrimButton.onClick.SetPersistentListenerState(2, UnityEventCallState.Off);
                break;
            case "failCombo":
                interrogateConfrimButton.onClick.SetPersistentListenerState(2, UnityEventCallState.Off);
                break;
            case "free":
                continueButton.onClick.SetPersistentListenerState(2, UnityEventCallState.Off);
                startCamTutButton.onClick.AddListener(camTutorial.StartCamTut);
                leaveButton.gameObject.SetActive(true);
                break;
            case "deduction":
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
        if(skipStates)
        {
            TutorialState.Next();
            skipStates = false;
        }
        print(TutorialState.Current);
    }

    [YarnCommand("MapArrow")]
    public void MapArrow()
    {
        arrow.SetActive(true);
    }
}
