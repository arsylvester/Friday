using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

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
    
    public InputFreeLookCam cam;
    public PlayerMovement movement;

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
        //Suppress items
        Interactable[] objs = FindObjectsOfType<Interactable>();
        foreach (Interactable obj in objs)
        {
            if(obj.GetComponent<Item>() && obj.GetComponent<Item>().Keyname != "manage_bills")
                obj.enabled = false;
        }

        //Suppress movement and all other interactions
        movement.enabled = false;
        cam.FreezeCamera();

        //Click bills step
        prompt.StartPrompt(DummyHighlight, BillsPrompt, true, null);
        hasBillsBeenSelected = false;
        while (!hasBillsBeenSelected)
        {
            yield return null;
        }
        hasBillsBeenSelected = false;
        prompt.EndPrompt();

        //Suppress exit and rotation
        InspectorAgent agent = GetComponent<InspectorAgent>();
        agent.SuppressExit = true;
        agent.SuppressRotation = true;

        //Record item step
        prompt.StartPrompt(DummyHighlight, RecordPrompt, true, null);
        hasRecordedItem = false;
        while(!hasRecordedItem)
        {
            yield return null;
        }
        hasRecordedItem = false;
        prompt.EndPrompt();

        //Unsuppress rotation
        agent.SuppressRotation = false;

        //Rotate item step
        prompt.StartPrompt(DummyHighlight, RotatePrompt, true, null);
        hasRotated = false;
        while(!hasRotated)
        {
            yield return null;
        }
        hasRotated = false;
        prompt.EndPrompt();

        //Record secret item step
        prompt.StartPrompt(DummyHighlight, SecretsPrompt, true, null);
        hasRecordedSecret = false;
        while(!hasRecordedSecret)
        {
            yield return null;
        }
        hasRecordedSecret = false;
        prompt.EndPrompt();

        //Unsuppress exit
        agent.SuppressExit = false;

        //Unsuppress items
        foreach(Interactable obj in objs)
        {
            obj.enabled = true;
        }

        //Leave item prompt
        prompt.StartPrompt(DummyHighlight, LeavePrompt, true, null);
        hasLeftItem = false;
        while(!hasLeftItem)
        {
            yield return null;
        }
        hasLeftItem = false;
        prompt.EndPrompt();
        Debug.Log(TutorialState.Current);
        TutorialState.Next();

        //Unsuppress camera and movement
        cam.UnfreezeCamera();
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

    private bool hasDoneCamTut = false;
    public void CamTut()
    {
        hasDoneCamTut = true;
    }
}
