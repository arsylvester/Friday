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
    [SerializeField] TutorialPrompt tutorialPrompt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator waitThenPrompt(GameObject highlight, GameObject currrPrompt)
    {
        yield return new WaitForSeconds(.5f);
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
                StartCoroutine(waitThenPrompt(convoAgainButton.gameObject, MM1Prompt));
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
            default:
                break;
        }
        tutorialPrompt.EndPrompt();
    }
}
