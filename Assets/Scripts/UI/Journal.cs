﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using UnityEngine.Events;

public class Journal : MonoBehaviour
{
    [SerializeField] GameObject itemJournal;
    [SerializeField] GameObject dialogJournal;
    [SerializeField] GameObject itemQuestioningBox;
    [SerializeField] Transform itemQuestioningContent;
    [SerializeField] Transform contentPanel;
    [SerializeField] Transform itemContentPanel;
    [SerializeField] List<Text> charSections;
    [SerializeField] List<Text> LocSections;
    [SerializeField] Text dialogText;
    [SerializeField] GameObject journalTextPrefab;
    [SerializeField] GameObject journalItemPrefab;
    [SerializeField] GameObject charSectionPrefab;
    [SerializeField] GameObject itemSectionPrefab;
    [SerializeField] GameObject deleteButton;
    [SerializeField] GameObject unhighlightAllButton;
    [SerializeField] GameObject markImportantButton;
    [SerializeField] GameObject Photograph;
    [SerializeField] int maxNumOfLinesSaveable = 50;
    [SerializeField] int maxNumOfEvidenceQuestioned = 2;
    [SerializeField] GameObject deductionElementPrefab;
    [SerializeField] Transform deductionPanel;
    [SerializeField] DeductionSummary deductionSummary;
    [SerializeField] DilogueTutorialManager dilogueTutorialManager;

    private int linesSaved = 0;
    private int NumOfEvidenceQuestioned = 0;
    private bool dialogSaveable = false;
    private bool keyDialog = false;
    private string keyName = "";
    private bool isQuestioning = false;
    private string keyText1 = "";
    private string keyText2 = "";
    private InMemoryVariableStorage varStorage;
    //public List<GameObject> highlightedText;
    //public List<GameObject> highlightedItems;
    public List<GameObject> highlightedEntries;

    public List<GameObject> keyEntries;

    public UnityEvent OnQuestionStart;
    public UnityEvent OnQuestionStop;

    DialogueCam dialogueCam;

    //TESTING
    //public Sprite testSprite1;
    public Sprite stockingSprite;

    // The dialogue runner that we want to attach the 'visited' function to
    [SerializeField] Yarn.Unity.DialogueRunner dialogueRunner;

    void Start()
    {
        varStorage = FindObjectOfType<InMemoryVariableStorage>();

        dialogueCam = FindObjectOfType<DialogueCam>();

        // Register a function on startup called "question" that lets Yarn
        // scripts check if questioned right or not.
        dialogueRunner.RegisterFunction("question", 2, delegate (Yarn.Value[] parameters)
        {
            bool correct1 = false, correct2 = false;
            foreach (GameObject question in highlightedEntries)
            {
                string currentKey = question.GetComponent<JournalElement>().keyID;
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
            if (correct1 && (correct2 || parameters[1].AsString == "-"))
            {
                print("That was correct!");
                return true;
            }
            return false;
        });

        //Deduction(string mainText, string summaryText, string key1, string key2, string key3, string key4)
        dialogueRunner.RegisterFunction("deduction", 6, delegate (Yarn.Value[] parameters)
        {
            var newDedElement = Instantiate(deductionElementPrefab, deductionPanel);
            newDedElement.GetComponent<DeductionElement>().SetUpDeduction(parameters[0].AsString, parameters[1].AsString, deductionSummary);

            List<GameObject> tempList = new List<GameObject>();
            for (int x = 0; x < keyEntries.Count; x++)
            {
                JournalElement currentJournalElement = keyEntries[x].GetComponent<JournalElement>();
                if(currentJournalElement != null)
                {
                    string currentKey = currentJournalElement.keyID;
                    if(currentKey == parameters[2].AsString ||
                        currentKey == parameters[3].AsString ||
                        currentKey == parameters[4].AsString ||
                        currentKey == parameters[5].AsString)
                    {
                        tempList.Add(keyEntries[x]);
                    }
                }
            }

            for(int y = 0; y < tempList.Count; y++)
            {
                GameObject temp = tempList[y];
                keyEntries.Remove(temp);
                Destroy(temp);
            }

            if(TutorialState.Current == "deduction")
            {
                dilogueTutorialManager.deductionButton = newDedElement.GetComponentInChildren<Button>();
                dilogueTutorialManager.startDeductionStep();
            }
        });

        //TEST:
        //SaveItem("Vase", "Its a container to hold flowers.", "Just a fancy, temporary flowerpot.", testSprite1, "KeyVase");
        SaveItem("Stocking Of Pennies", "The murder weapon.", "What a crude way to kill such an innocent woman.", stockingSprite, "pennies");
        CloseJournals();
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
        if (Input.GetKeyDown(KeyCode.F) && dialogSaveable && linesSaved < maxNumOfLinesSaveable && dilogueTutorialManager.canSaveDialogue)
        {
            SaveDialogue();
        }
    }

    private void SaveDialogue()
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
            newJournalText.GetComponent<DialogueJournalElement>().keyID = keyName;
            keyEntries.Add(newJournalText);
        }
        linesSaved++;
        CanSaveDialogue(false);

        if(TutorialState.Current == "recordDialogue")
        {
            dilogueTutorialManager.EndTutorial();
            dilogueTutorialManager.fridayAloneButton = newJournalText.GetComponent<Button>();
        }
    }

    public void SaveItem(string itemName, string desc, string flavor, Sprite sprite, string keyID)
    {
        OpenJournals();
        string location = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        print(location);
        Transform locSubsection = null;
        //Find the location subsection
        foreach (Text name in LocSections)
        {
            if (location.ToLower() == name.text.ToLower())
            {
                locSubsection = name.transform;
                break;
            }
        }
        //If location is not already in the journal;
        if (locSubsection == null)
        {
            locSubsection = Instantiate(itemSectionPrefab, itemContentPanel).transform;
            locSubsection.GetComponentInChildren<Text>().text = location;
            LocSections.Add(locSubsection.GetComponent<Text>());
        }
        //Check if item has been saved before or not. Delete it if so.
        foreach (ItemJournalElement element in locSubsection.GetComponentsInChildren<ItemJournalElement>())
        {
            if (itemName == element.itemName)
            {
                Destroy(element.gameObject);
                break;
            }
        }
        //Create the item in the journal
        GameObject newJournalItem = Instantiate(journalItemPrefab, locSubsection);
        newJournalItem.GetComponent<ItemJournalElement>().SetUpEntry(itemName, desc, flavor, sprite, keyID, locSubsection, Photograph.GetComponent<ItemPhotograph>());

        if(keyID != "" && keyID != null)
        {
            keyEntries.Add(newJournalItem);
        }

        if(keyID == "pennies")
        {
            dilogueTutorialManager.stockingButton = newJournalItem.GetComponent<Button>();
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
        StartCoroutine(OpenJournalsAfterFade());
    }

    IEnumerator OpenJournalsAfterFade()
    {
        yield return new WaitForSeconds(0.11f);
        // wait for fade on entering dialogue
        if (!dialogueCam.isDoneTalking)
        {
            yield return new WaitForSeconds(1);
            itemJournal.SetActive(true);
            dialogJournal.SetActive(true);
        }
        else
        {
            itemJournal.SetActive(true);
            dialogJournal.SetActive(true);
        }
    }

    IEnumerator StartDialogueAfterFade()
    {
        yield return new WaitForSeconds(1);
    }

    //Close both panels. Meant for when dialogue ends.
    public void CloseJournals()
    {
        itemJournal.SetActive(false);
        dialogJournal.SetActive(false);
    }

    public void AddHighlighted(GameObject entry)
    {
            
        if (isQuestioning)
        {
            if (NumOfEvidenceQuestioned < maxNumOfEvidenceQuestioned)
            {
                highlightedEntries.Add(entry);
                entry.GetComponent<JournalElement>().Highlight();
                entry.transform.SetParent(itemQuestioningContent);
                NumOfEvidenceQuestioned++;
            }
        }
        else
        {
            highlightedEntries.Add(entry);
            entry.GetComponent<JournalElement>().Highlight();
            if (!deleteButton.activeInHierarchy)
            {
                ToggleButtonsOn();
            }
        }
    }

    public void RemoveHighlighted(GameObject entry)
    {
            highlightedEntries.Remove(entry);
            entry.GetComponent<JournalElement>().Unhighlight();
            if (isQuestioning)
            {
                entry.transform.SetParent(entry.GetComponent<JournalElement>().journalParent);
                NumOfEvidenceQuestioned--;
            }
            else if (highlightedEntries.Count <= 0)
            {
                ToggleButtonsOff();
            }
    }

    public void DeleteAllHighlighted()
    {
        foreach (GameObject lit in highlightedEntries)
        {
            if(lit.GetComponent<JournalElement>().keyID != "pennies" && lit.GetComponent<JournalElement>().keyID != "friday_alone")
            {
                linesSaved--;
                Destroy(lit.gameObject);
            }
            else
            {
                lit.GetComponent<JournalElement>().Unhighlight();
                print("Hey, you can't delete that!");
            }
        }
        highlightedEntries.Clear();
        ToggleButtonsOff();
    }

    public void MarkImportant()
    {
        foreach (GameObject lit in highlightedEntries)
        {
            lit.GetComponent<JournalElement>().MarkImportant();
        }
        UnhighlightAll();
    }

        public void UnhighlightAll()
    {
        if (isQuestioning)
        {
            foreach (GameObject lit in highlightedEntries)
            {
                lit.transform.SetParent(lit.GetComponent<JournalElement>().journalParent);
                lit.GetComponent<JournalElement>().Unhighlight();
            }
            NumOfEvidenceQuestioned = 0;
            highlightedEntries.Clear();
        }
        else
        {
            foreach (GameObject lit in highlightedEntries)
            {
                lit.GetComponent<JournalElement>().Unhighlight();
            }
        }
        highlightedEntries.Clear();
        ToggleButtonsOff();
    }

    private void ToggleButtonsOn()
    {
        deleteButton.SetActive(true);
        unhighlightAllButton.SetActive(true);
        markImportantButton.SetActive(true);
    }

    private void ToggleButtonsOff()
    {
        deleteButton.SetActive(false);
        unhighlightAllButton.SetActive(false);
        markImportantButton.SetActive(false);
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
        OnQuestionStart.Invoke();
        isQuestioning = true;
        OpenJournals();
        itemQuestioningBox.SetActive(true);
        UnhighlightAll();
    }

    [YarnCommand("stopquestioning")]
    public void StopQuestioning()
    {
        OnQuestionStop.Invoke();
        UnhighlightAll();
        isQuestioning = false;
        itemQuestioningBox.SetActive(false);
    }
/*
    [YarnCommand("deduction")]
    public void Deduction(string mainText, string summaryText, string key1, string key2, string key3, string key4)
    {
        for (int x = 0; x < keyEntries.Count; x++)
            {
                string currentKey = keyEntries[x].GetComponent<JournalElement>().keyID;
                if(currentKey == key1 ||
                    currentKey == key2 ||
                    currentKey == key3 ||
                    currentKey == key4)
                {
                    GameObject temp = keyEntries[x];
                    keyEntries.Remove(temp);
                    Destroy(temp);
                }
            }
        var newDedElement = Instantiate(deductionElementPrefab, deductionPanel);
        newDedElement.GetComponent<DeductionElement>().SetUpDeduction(mainText, summaryText);
    }*/
}
