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
    public Button deductionButton;
    
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

    private bool stockingPressed = false;
    private bool fridayAlonePressed = false;
    private bool deductionPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator interrogatePrompts()
    {
        tutorialPrompt.StartPrompt(interHighlight, interrogatePrompt1, true);

        while(!Input.GetMouseButtonDown(0))
        {
             yield return null;
        }

        interrogatePrompt1.SetActive(false);
        interrogatePrompt2.SetActive(true);

        yield return new WaitForEndOfFrame();

        while(!Input.GetMouseButtonDown(0))
        {
             yield return null;
        }

        interrogatePrompt2.SetActive(false);
        interrogatePrompt3.SetActive(true);

        yield return new WaitForEndOfFrame();

        while(!Input.GetMouseButtonDown(0))
        {
             yield return null;
        }

        interrogatePrompt3.SetActive(false);
        interrogatePrompt4.SetActive(true);

        yield return new WaitForEndOfFrame();

        while(!Input.GetMouseButtonDown(0))
        {
             yield return null;
        }

        interrogatePrompt4.SetActive(false);

        EndTutorial();
        //prompt();
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

    IEnumerator comboAttempt()
    {
        tutorialPrompt.StartPrompt(fridayAloneButton.gameObject, comboAttemptPrompt, false);
        tutorialPrompt.StartPrompt(stockingButton.gameObject, comboAttemptPrompt, false);

        fridayAloneButton.onClick.AddListener(fridayClicked);
        stockingButton.onClick.AddListener(stockingClicked);

        fridayAlonePressed = false;
        stockingPressed = false;

        while(!fridayAlonePressed || !stockingPressed)
        {
            yield return new WaitForEndOfFrame();
        }

        tutorialPrompt.StartPrompt(interrogateConfrimButton.gameObject, comboAttemptPrompt, true);
        interrogateConfrimButton.onClick.SetPersistentListenerState(2, UnityEventCallState.RuntimeOnly);
    }

    IEnumerator stockingAttempt()
    {
        yield return new WaitForSeconds(.1f);
        tutorialPrompt.StartPrompt(stockingButton.gameObject, stockingPrompt, false);

        //stockingButton.onClick.AddListener(stockingClicked);

        stockingPressed = false;

        while(!stockingPressed)
        {
            yield return new WaitForEndOfFrame();
        }
        tutorialPrompt.StartPrompt(interrogateConfrimButton.gameObject, stockingPrompt, true);
        interrogateConfrimButton.onClick.SetPersistentListenerState(2, UnityEventCallState.RuntimeOnly);
    }

    public void startDeductionStep()
    {
        StartCoroutine(deductionStep());
    }
    
    IEnumerator deductionStep()
    {
        yield return new WaitForSeconds(.1f);
        tutorialPrompt.StartPrompt(deductionButton.gameObject, deduectionPrompt, true);
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
                continueButton.onClick.SetPersistentListenerState(0, UnityEventCallState.Off);
                leaveButton.gameObject.SetActive(true);
                StartCoroutine(waitThenPrompt(leaveButton.gameObject, leavePrompt, 1f));
                break;
            case "followUpQuestions":
                StartCoroutine(waitThenPrompt(followUpQButton.gameObject, FUQPrompt, .05f));
                break;
            case "recordDialogue":
                continueButton.onClick.SetPersistentListenerState(0, UnityEventCallState.Off);
                tutorialPrompt.StartPrompt(recDiaHighlight, recDiaPrompt, true);
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
        print("Ending Tutorial");
        switch(TutorialState.Current)
        {
            case "mainMenu1":
                convoAgainButton.onClick.SetPersistentListenerState(2, UnityEventCallState.Off);
                break;
             case "leaveConvo":
                leaveButton.onClick.SetPersistentListenerState(4, UnityEventCallState.Off);
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
        TutorialState.Next();
        TutorialState.Next();
        print(TutorialState.Current);
    }
}
