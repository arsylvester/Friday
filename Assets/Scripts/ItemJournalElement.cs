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
    }

    public void Clicked()
    {
        isHighlighted = !isHighlighted;

        if (isHighlighted)
        {
            button.colors = highlightedColors;
            journal.AddHighlighted(gameObject);
        }
        else
        {
            button.colors = ColorBlock.defaultColorBlock;
            journal.RemoveHighlighted(gameObject);
        }
    }

    public void Unhighlight()
    {
        isHighlighted = false;
        button.colors = ColorBlock.defaultColorBlock;
    }
}
