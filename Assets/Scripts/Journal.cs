using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class Journal : MonoBehaviour
{
    [SerializeField] GameObject itemJournal;
    [SerializeField] GameObject dialogJournal;
    [SerializeField] Transform contentPanel;
    [SerializeField] List<Text> charSections;
    [SerializeField] Text dialogText;
    [SerializeField] GameObject journalTextPrefab;
    [SerializeField] GameObject charSectionPrefab;

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
            string savedText = dialogText.text;
            string speaker = savedText.Substring(0, savedText.IndexOf(':'));
            Transform character = null;
            //Find the character subsection
            foreach(Text name in charSections)
            {
                if(speaker.ToLower() == name.text.ToLower())
                {
                    character = name.transform;
                    break;
                }
            }
            //If character is not already in the journal;
            if(character == null)
            {
                character = Instantiate(charSectionPrefab, contentPanel).transform;
                character.GetComponentInChildren<Text>().text = speaker;
                charSections.Add(character.GetComponent<Text>());
            }
            //Check if text has been saved before or not. Delete it if so.
            foreach(Text dialog in character.GetComponentsInChildren<Text>())
            {
                if(savedText == dialog.text)
                {
                    Destroy(dialog.transform.parent.gameObject);
                    break;
                }
            }
            //Create the dialog in the journal
            GameObject newJournalText =  Instantiate(journalTextPrefab, character);
            newJournalText.GetComponentInChildren<Text>().text = savedText;
            //Mark it as important if key.
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
