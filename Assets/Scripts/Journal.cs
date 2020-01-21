using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class Journal : MonoBehaviour
{
    [SerializeField] GameObject itemJournal;
    [SerializeField] GameObject dialogJournal;
    [SerializeField] Text dialogText;
    [SerializeField] GameObject journalTextPrefab;

    private bool dialogSaveable = false;
    private bool keyDialog = false;
    private string keyName = "";

    void Start()
    {
        itemJournal.SetActive(false);
        dialogJournal.SetActive(false);
    }

    void Update()
    {
        // Open Journal panels
        if(Input.GetKeyDown(KeyCode.Q))
        {
            itemJournal.SetActive(!itemJournal.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            dialogJournal.SetActive(!dialogJournal.activeSelf);
        }

        //Add dialogue to journal
        if(dialogSaveable && Input.GetKeyDown(KeyCode.F))
        {
            GameObject newJournalText =  Instantiate(journalTextPrefab, dialogJournal.transform);
            newJournalText.GetComponentInChildren<Text>().text = dialogText.text;
            if (keyDialog)
            {
                newJournalText.GetComponent<DialogueJournalElement>().isKeyDialogue = true;
                newJournalText.GetComponent<DialogueJournalElement>().keyName = keyName;
            }
            CanSaveDialogue(false);
        }
    }

    //Toggle the ability to save current dialog to the journal.
    public void CanSaveDialogue(bool canSave)
    {
        dialogSaveable = canSave;
    }

    //Open both panels. Meant for when dialogue begins.
    public void OpenJournals()
    {
        itemJournal.SetActive(true);
        dialogJournal.SetActive(true);
    }

    [YarnCommand("startkey")]
    public void StartKeyDialogue(string s)
    {
        keyDialog = true;
        keyName = s;
    }

    [YarnCommand("endkey")]
    public void EndKeyDialogue()
    {
        keyDialog = false;
        keyName = "";
    }
}
