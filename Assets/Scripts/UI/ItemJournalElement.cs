using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemJournalElement : JournalElement
{
    public string itemName = "";
    public string description = "";
    public string flavorText = "";
    //public string keyID = "";
    public Sprite itemPhoto;

    [SerializeField] Text nameTextbox;
    [SerializeField] Text descTextbox;
    [SerializeField] Image itemImageBox;
    [SerializeField] GameObject markedImage;

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

    public override void Highlight()
    {
        isHighlighted = true;
        unMarkedColors = button.colors;
        button.colors = highlightedColors;
    }

    public override void Unhighlight()
    {
        isHighlighted = false;
        button.colors = unMarkedColors;
    }

    public override void MarkImportant()
    {
        markedImage.SetActive(!markedImage.activeInHierarchy);
    }

    public void ImageClicked()
    {
        itemPhotograph.ExpandImage(itemPhoto, flavorText);
    }
}
