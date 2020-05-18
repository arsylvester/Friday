using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ItemJournalElement : JournalElement
{
    public string itemName = "";
    public string description = "";
    public string flavorText = "";
    public string location = "";
    //public string keyID = "";
    public Sprite itemPhoto;

    [SerializeField] Text nameTextbox;
    [SerializeField] Text descTextbox;
    [SerializeField] Image itemImageBox;
    [SerializeField] GameObject markedImage;
    [SerializeField] GameObject markImportantButton;
    [SerializeField] GameObject deleteButton;

   // public ColorBlock highlightedColors = ColorBlock.defaultColorBlock;
   // public Transform journalParent;

    private Button button;
    private Journal journal;
    private ItemPhotograph itemPhotograph;
    private ColorBlock unMarkedColors;
   // private bool isHighlighted = false;

    private void Awake()
    {
        button = GetComponent<Button>();
        journal = GetComponentInParent<Journal>();
        itemPhotograph = FindObjectOfType<ItemPhotograph>();
    }

    public void SetUpEntry(string name, string desc, string flavor, Sprite sprite, string ID, Transform parent, ItemPhotograph photo)
    {
        itemName = name;
        description = desc;
        flavorText = flavor;
        itemPhoto = sprite;
        keyID = ID;
        journalParent = parent;
        itemPhotograph = photo;
        location = SceneManager.GetActiveScene().name;

        nameTextbox.text = itemName;
        descTextbox.text = description;
        itemImageBox.sprite = itemPhoto;
    }

    public override void Clicked()
    {
        if (!isHighlighted)
        {
            journal.AddHighlighted(gameObject);
        }
        else
        {
            journal.RemoveHighlighted(gameObject);
        }
    }

    public override void Highlight(bool buttons)
    {
        isHighlighted = true;
        // unMarkedColors = button.colors;
        // button.colors = highlightedColors;
        if (buttons)
        {
            markImportantButton.SetActive(true);
            deleteButton.SetActive(true);
        }
    }

    public override void Unhighlight()
    {
        isHighlighted = false;
       // button.colors = unMarkedColors;
        markImportantButton.SetActive(false);
        deleteButton.SetActive(false);
    }

    public override void MarkImportant()
    {
        markedImage.SetActive(!markedImage.activeInHierarchy);
    }

    public void ImageClicked()
    {
        itemPhotograph.ExpandImage(itemPhoto, flavorText, itemName, location);
    }

    public override void DeleteEntry()
    {
        journal.ConfirmDeletion();
    }
}
