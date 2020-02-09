using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueJournalElement : JournalElement
{
    //public string keyName = "";
    //public bool isHighlighted = false;
   // public ColorBlock highlightedColors = ColorBlock.defaultColorBlock;
   // public Transform journalParent;
    public RectTransform textRect;
    public Color markedColor;

    private Button button;
    private Journal journal;
    private Text text;
   // private bool isMarked = false;

    private void Awake()
    {
        button = GetComponent<Button>();
        journal = GetComponentInParent<Journal>();
        text = GetComponentInChildren<Text>();
    }

    public override void Clicked()
    {
        if (!isHighlighted)
        {
            journal.AddHighlighted(gameObject, true);
        }
        else
        {
            journal.RemoveHighlighted(gameObject);
        }
    }

    public override void Highlight()
    {
        isHighlighted = true;
        button.colors = highlightedColors;
    }

    public override void Unhighlight()
    {
        isHighlighted = false;
        button.colors = ColorBlock.defaultColorBlock;
    }

    public override void MarkImportant()
    {
        if (isMarked)
        {
            text.fontStyle = FontStyle.Normal;
            text.color = Color.black;
        }
        else
        {
            text.fontStyle = FontStyle.BoldAndItalic;
            text.color = markedColor;
        }
        isMarked = !isMarked;
    }
}
