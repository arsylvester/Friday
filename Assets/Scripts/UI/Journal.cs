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
    //public List<GameObject> highlightedText;
    //public List<GameObject> highlightedItems;
    public List<GameObject> highlightedEntries;

    public UnityEvent OnQuestionStart;
    public UnityEvent OnQuestionStop;

    //TESTING
    public Sprite testSprite1;
    public Sprite testSprite2;

    // The dialogue runner that we want to attach the 'visited' function to
    [SerializeField] Yarn.Unity.DialogueRunner dialogueRunner;

    void Start()
    {
        varStorage = FindObjectOfType<InMemoryVariableStorage>();

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

        //TEST:
        SaveItem("Vase", "Its a container to hold flowers.", "Just a fancy, temporary flowerpot.", testSprite1, "KeyVase");
        SaveItem("Yarn", "A ball of yarn.", "What differentiates yarn, string, twine, and rope?", testSprite2, "KeyYarn");
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
            newJournalText.GetComponent<DialogueJournalElement>().keyID = keyName;
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
                entry.transform.SetParent(dialogQuestioningContent);
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
            linesSaved--;
            Destroy(lit);
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
