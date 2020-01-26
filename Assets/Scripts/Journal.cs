using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class Journal : MonoBehaviour
{
    [SerializeField] GameObject itemJournal;
    [SerializeField] GameObject dialogJournal;
    [SerializeField] GameObject dialogQuestioningBox;
    [SerializeField] Transform dialogQuestioningContent;
    [SerializeField] Transform contentPanel;
    [SerializeField] List<Text> charSections;
    [SerializeField] Text dialogText;
    [SerializeField] GameObject journalTextPrefab;
    [SerializeField] GameObject charSectionPrefab;
    [SerializeField] GameObject deleteButton;
    [SerializeField] GameObject unhighlightAllButton;

    private bool dialogSaveable = false;
    private bool keyDialog = false;
    private string keyName = "";
    private bool isQuestioning = false;
    private string keyText1 = "";
    private string keyText2 = "";
    private InMemoryVariableStorage varStorage;
    public List<GameObject> highlightedText;

    // The dialogue runner that we want to attach the 'visited' function to
    [SerializeField] Yarn.Unity.DialogueRunner dialogueRunner;

    void Start()
    {
        itemJournal.SetActive(false);
        dialogJournal.SetActive(false);
        varStorage = FindObjectOfType<InMemoryVariableStorage>();

        // Register a function on startup called "question" that lets Yarn
        // scripts check if questioned right or not.
        dialogueRunner.RegisterFunction("question", 2, delegate (Yarn.Value[] parameters)
        {
            bool correct1 = false, correct2 = false;
            foreach (GameObject question in highlightedText)
            {
                string currentKey = question.GetComponent<DialogueJournalElement>().keyName;
                if (currentKey == parameters[0].AsString)
                {
                    correct1 = true;
                }
                else if (currentKey == parameters[1].AsString)
                {
                    correct2 = true;
                }
                else
                {
                    return false;
                }
            }
            if (correct1 && correct2)
            {
                print("That was correct!");
                return true;
            }
            return false;
        });
    }

    void Update()
    {
        // Open Journal panels
        if (Input.GetKeyDown(KeyCode.Q) && !isQuestioning)
        {
            itemJournal.SetActive(!itemJournal.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.E) && !isQuestioning)
        {
            dialogJournal.SetActive(!dialogJournal.activeSelf);
            UnhighlightAll();
        }

        //Add dialogue to journal
        if (dialogSaveable && Input.GetKeyDown(KeyCode.F))
        {
            string savedText = dialogText.text;
            string speaker = savedText.Substring(0, savedText.IndexOf(':'));
            Transform character = null;
            //Find the character subsection
            foreach (Text name in charSections)
            {
                if (speaker.ToLower() == name.text.ToLower())
                {
                    character = name.transform;
                    break;
                }
            }
            //If character is not already in the journal;
            if (character == null)
            {
                character = Instantiate(charSectionPrefab, contentPanel).transform;
                character.GetComponentInChildren<Text>().text = speaker;
                charSections.Add(character.GetComponent<Text>());
            }
            //Check if text has been saved before or not. Delete it if so.
            foreach (Text dialog in character.GetComponentsInChildren<Text>())
            {
                if (savedText == dialog.text)
                {
                    Destroy(dialog.transform.parent.gameObject);
                    break;
                }
            }
            //Create the dialog in the journal
            GameObject newJournalText = Instantiate(journalTextPrefab, character);
            newJournalText.GetComponentInChildren<Text>().text = savedText;
            newJournalText.GetComponent<DialogueJournalElement>().journalParent = character;
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

    public void AddHighlighted(GameObject entry)
    {
        highlightedText.Add(entry);
        if(isQuestioning)
        {
            entry.transform.SetParent(dialogQuestioningContent);
        }
        else if (!deleteButton.activeInHierarchy)
        {
            deleteButton.SetActive(true);
            unhighlightAllButton.SetActive(true);
        }
    }

    public void RemoveHighlighted(GameObject entry)
    {
        highlightedText.Remove(entry);
        if (isQuestioning)
        {
            entry.transform.SetParent(entry.GetComponent<DialogueJournalElement>().journalParent);
        }
        else if (highlightedText.Count <= 0)
        {
            deleteButton.SetActive(false);
            unhighlightAllButton.SetActive(false);
        }
    }

    public void DeleteAllHighlighted()
    {
        foreach (GameObject lit in highlightedText)
        {
            Destroy(lit);
        }
        highlightedText.Clear();
        deleteButton.SetActive(false);
        unhighlightAllButton.SetActive(false);
    }

    public void UnhighlightAll()
    {
        if(isQuestioning)
        {
            foreach (GameObject lit in highlightedText)
            {
                lit.transform.SetParent(lit.GetComponent<DialogueJournalElement>().journalParent);
            }
        }
        foreach (GameObject lit in highlightedText)
        {
            lit.GetComponent<DialogueJournalElement>().Unhighlight();
        }
        highlightedText.Clear();
        deleteButton.SetActive(false);
        unhighlightAllButton.SetActive(false);
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

    [YarnCommand("startquestioning")]
    public void StartQuestioning()
    {
        isQuestioning = true;
        dialogJournal.SetActive(true);
        dialogQuestioningBox.SetActive(true);
        UnhighlightAll();
    }

    [YarnCommand("stopquestioning")]
    public void StopQuestioning()
    {
        UnhighlightAll();
        isQuestioning = false;
        dialogQuestioningBox.SetActive(false);
    }
}
