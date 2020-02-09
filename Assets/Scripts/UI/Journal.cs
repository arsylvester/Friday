using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using UnityEngine.Events;

public class Journal : MonoBehaviour
{
    [SerializeField] GameObject itemJournal;
    [SerializeField] GameObject dialogJournal;
    [SerializeField] GameObject dialogQuestioningBox;
    [SerializeField] Transform dialogQuestioningContent;
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
    [SerializeField] GameObject deleteButton;
    [SerializeField] GameObject unhighlightAllButton;
    [SerializeField] GameObject markImportantButton;
    [SerializeField] GameObject Photograph;
    [SerializeField] int maxNumOfLinesSaveable = 50;
    [SerializeField] int maxNumOfEvidenceQuestioned = 2;

    private int linesSaved = 0;
    private int NumOfEvidenceQuestioned = 0;
    private bool dialogSaveable = false;
    private bool keyDialog = false;
    private string keyName = "";
    private bool isQuestioning = false;
    private string keyText1 = "";
    private string keyText2 = "";
    private InMemoryVariableStorage varStorage;
    public List<GameObject> highlightedText;
    public List<GameObject> highlightedItems;

    public UnityEvent OnQuestionStart;
    public UnityEvent OnQuestionStop;

    //TESTING
    public Sprite testSprite1;
    public Sprite testSprite2;

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
            foreach (GameObject question in highlightedItems)
            {
                string currentKey = question.GetComponent<ItemJournalElement>().keyID;
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

        //TEST:
        SaveItem("Vase", "Its a container to hold flowers.", "Just a fancy, temporary flowerpot.", testSprite1, "KeyVase");
        SaveItem("Yarn", "A ball of yarn.", "What differentiates yarn, string, twine, and rope?", testSprite2, "KeyYarn");
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
        if (dialogSaveable && linesSaved < maxNumOfLinesSaveable && Input.GetKeyDown(KeyCode.F))
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
            newJournalText.GetComponent<DialogueJournalElement>().isKeyDialogue = true;
            newJournalText.GetComponent<DialogueJournalElement>().keyName = keyName;
        }
        linesSaved++;
        CanSaveDialogue(false);
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
            locSubsection = Instantiate(charSectionPrefab, itemContentPanel).transform;
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

    public void AddHighlighted(GameObject entry, bool isDialogue)
    {
        if (isDialogue)
        {
            highlightedText.Add(entry);
            if (isQuestioning)
            {
                if (NumOfEvidenceQuestioned < maxNumOfEvidenceQuestioned)
                {
                    entry.transform.SetParent(dialogQuestioningContent);
                    NumOfEvidenceQuestioned++;

                }
                else
                {
                    entry.GetComponent<DialogueJournalElement>().Unhighlight();
                    highlightedText.Remove(entry);
                }
            }
            else if (!deleteButton.activeInHierarchy)
            {
                ToggleButtonsOn();
            }
        }
        //Is an item
        else
        {
            if (isQuestioning)
            {
                if (NumOfEvidenceQuestioned < maxNumOfEvidenceQuestioned)
                {
                    highlightedItems.Add(entry);
                    entry.GetComponent<ItemJournalElement>().Highlight();
                    entry.transform.SetParent(itemQuestioningContent);
                    NumOfEvidenceQuestioned++;
                }
            }
        }
    }

    public void RemoveHighlighted(GameObject entry, bool isDialogue)
    {
        if (isDialogue)
        {
            highlightedText.Remove(entry);
            if (isQuestioning)
            {
                entry.transform.SetParent(entry.GetComponent<DialogueJournalElement>().journalParent);
                NumOfEvidenceQuestioned--;
            }
            else if (highlightedText.Count <= 0)
            {
                ToggleButtonsOff();
            }
        }
        //Is an item
        else
        {
            if (isQuestioning)
            {
                highlightedItems.Remove(entry);
                entry.GetComponent<ItemJournalElement>().Unhighlight();
                entry.transform.SetParent(entry.GetComponent<ItemJournalElement>().journalParent);
                NumOfEvidenceQuestioned--;
            }
        }
    }

    public void DeleteAllHighlighted()
    {
        foreach (GameObject lit in highlightedText)
        {
            linesSaved--;
            Destroy(lit);
        }
        highlightedText.Clear();
        ToggleButtonsOff();
    }

    public void MarkImportant()
    {
        foreach (GameObject lit in highlightedText)
        {
            lit.GetComponent<DialogueJournalElement>().MarkImportant();
        }
        UnhighlightAll();
    }

        public void UnhighlightAll()
    {
        if (isQuestioning)
        {
            foreach (GameObject lit in highlightedText)
            {
                lit.transform.SetParent(lit.GetComponent<DialogueJournalElement>().journalParent);
                lit.GetComponent<DialogueJournalElement>().Unhighlight();
            }
            foreach (GameObject item in highlightedItems)
            {
                item.transform.SetParent(item.GetComponent<ItemJournalElement>().journalParent);
                item.GetComponent<ItemJournalElement>().Unhighlight();
            }
            NumOfEvidenceQuestioned = 0;
            highlightedItems.Clear();
        }
        else
        {
            foreach (GameObject lit in highlightedText)
            {
                lit.GetComponent<DialogueJournalElement>().Unhighlight();
            }
        }
        highlightedText.Clear();
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
        dialogQuestioningBox.SetActive(true);
        itemQuestioningBox.SetActive(true);
        UnhighlightAll();
    }

    [YarnCommand("stopquestioning")]
    public void StopQuestioning()
    {
        OnQuestionStop.Invoke();
        UnhighlightAll();
        isQuestioning = false;
        dialogQuestioningBox.SetActive(false);
        itemQuestioningBox.SetActive(false);
    }
}
