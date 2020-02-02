using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemJournalElement : MonoBehaviour
{
    public string itemName = "";
    public string description = "";
    public string flavorText = "";
    public string keyID = "";
    public Sprite itemPhoto;

    [SerializeField] Text nameTextbox;
    [SerializeField] Text descTextbox;
    [SerializeField] Image itemImageBox;

    public ColorBlock highlightedColors = ColorBlock.defaultColorBlock;
    public Transform journalParent;

    private Button button;
    private Journal journal;
    private bool isHighlighted = false;

    private void Awake()
    {
        button = GetComponent<Button>();
        journal = GetComponentInParent<Journal>();
    }

    public void SetUpEntry(string name, string desc, string flavor, Sprite sprite, string ID, Transform parent)
    {
        itemName = name;
        description = desc;
        flavorText = flavor;
        itemPhoto = sprite;
        keyID = ID;
        journalParent = parent;

        nameTextbox.text = itemName;
        descTextbox.text = description;
        itemImageBox.sprite = itemPhoto;
    }

    public void Clicked()
    {
        if (!isHighlighted)
        {
            journal.AddHighlighted(gameObject, false);
        }
        else
        {
            journal.RemoveHighlighted(gameObject, false);
        }
    }

    public void Highlight()
    {
        isHighlighted = true;
        button.colors = highlightedColors;
    }

    public void Unhighlight()
    {
        isHighlighted = false;
        button.colors = ColorBlock.defaultColorBlock;
    }
}
