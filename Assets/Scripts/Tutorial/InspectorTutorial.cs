using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectorTutorial : MonoBehaviour
{
    public TutorialPrompt prompt;
    private bool started = false;

    public GameObject BillsPrompt;
    public GameObject RecordPrompt;
    public GameObject RotatePrompt;
    public GameObject SecretsPrompt;
    public GameObject LeavePrompt;

    public GameObject DummyHighlight;
    /*
    private void Start()
    {
        InvokeRepeating("PrintState", 0, 1);
    }

    void PrintState()
    {
        Debug.Log(TutorialState.Current);
    }
    */
    // Update is called once per frame
    void Update()
    {
        if(TutorialState.Current == "inspectorTutorial" && !started)
        {
            started = true;
            StartCoroutine(InspectorTutorialSequence());
        }
    }

    IEnumerator InspectorTutorialSequence()
    {
        //Click bills step
        prompt.StartPrompt(DummyHighlight, BillsPrompt, true);
        yield return new WaitForSeconds(1);
        while (!hasBillsBeenSelected)
        {
            yield return null;
        }
        hasBillsBeenSelected = false;
        prompt.EndPrompt();

        //Record item step
        prompt.StartPrompt(DummyHighlight, RecordPrompt, true);
        yield return new WaitForSeconds(1);
        prompt.EndPrompt();

        //Rotate item step
        prompt.StartPrompt(DummyHighlight, RotatePrompt, true);
        yield return new WaitForSeconds(1);
        prompt.EndPrompt();

        //Record secret item step
        prompt.StartPrompt(DummyHighlight, SecretsPrompt, true);
        yield return new WaitForSeconds(1);
        prompt.EndPrompt();

        //Leave item prompt
        prompt.StartPrompt(DummyHighlight, LeavePrompt, true);
        yield return new WaitForSeconds(1);
        prompt.EndPrompt();

        TutorialState.Next();
    }


    private bool hasBillsBeenSelected = false;
    public void SelectBills()
    {
        hasBillsBeenSelected = true;
    }
}
