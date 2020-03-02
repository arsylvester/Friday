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
        hasBillsBeenSelected = false;
        while (!hasBillsBeenSelected)
        {
            yield return null;
        }
        hasBillsBeenSelected = false;
        prompt.EndPrompt();
        yield return new WaitForSeconds(0.1F);

        //Suppress exit and rotation
        InspectorAgent agent = GetComponent<InspectorAgent>();
        agent.SuppressExit = true;
        agent.SuppressRotation = true;

        //Record item step
        prompt.StartPrompt(DummyHighlight, RecordPrompt, true);
        hasRecordedItem = false;
        while(!hasRecordedItem)
        {
            yield return null;
        }
        hasRecordedItem = false;
        prompt.EndPrompt();
        yield return new WaitForSeconds(0.1F);

        //Unsuppress rotation
        agent.SuppressRotation = false;

        //Rotate item step
        prompt.StartPrompt(DummyHighlight, RotatePrompt, true);
        hasRotated = false;
        while(!hasRotated)
        {
            yield return null;
        }
        hasRotated = false;
        prompt.EndPrompt();
        yield return new WaitForSeconds(0.1F);

        //Record secret item step
        prompt.StartPrompt(DummyHighlight, SecretsPrompt, true);
        hasRecordedSecret = false;
        while(!hasRecordedSecret)
        {
            yield return null;
        }
        hasRecordedSecret = false;
        prompt.EndPrompt();
        yield return new WaitForSeconds(0.1F);

        //Unsuppress exit
        agent.SuppressExit = false;

        //Leave item prompt
        prompt.StartPrompt(DummyHighlight, LeavePrompt, true);
        hasLeftItem = false;
        while(!hasLeftItem)
        {
            yield return null;
        }
        hasLeftItem = false;
        prompt.EndPrompt();
        yield return new WaitForSeconds(0.1F);
        Debug.Log(TutorialState.Current);
        TutorialState.Next();
    }


    private bool hasBillsBeenSelected = false;
    public void SelectBills()
    {
        hasBillsBeenSelected = true;
    }

    private bool hasRecordedItem = false;
    public void RecordItem()
    {
        hasRecordedItem = true;
    }

    private bool hasRotated = false;
    public void RotateItem()
    {
        hasRotated = true;
    }

    private bool hasRecordedSecret = false;
    public void RecordSecret()
    {
        hasRecordedSecret = true;
    }

    private bool hasLeftItem = false;
    public void LeaveItem()
    {
        hasLeftItem = true;
    }
}
