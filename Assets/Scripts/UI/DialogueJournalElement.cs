using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueJournalElement : MonoBehaviour
{
    public bool isKeyDialogue = false;
    public string keyName = "";
    public bool isHighlighted = false;
    public ColorBlock highlightedColors = ColorBlock.defaultColorBlock;
    public Transform journalParent;
    public RectTransform textRect;
    public Color markedColor;

    private Button button;
    private Journal journal;
    private Text text;
    private bool isMarked = false;

    private void Awake()
    {
        button = GetComponent<Button>();
        journal = GetComponentInParent<Journal>();
        text = GetComponentInChildren<Text>();
        //GetComponent<RectTransform>().rect.height = textRect.rect.height
    }

    public void Clicked()
    {
        isHighlighted = !isHighlighted;

        if(isHighlighted)
        {
            button.colors = highlightedColors;
            //journal.highlightedText.Add(gameObject);
            journal.AddHighlighted(gameObject, true);
        }
        else
        {
            button.colors = ColorBlock.defaultColorBlock;
            journal.RemoveHighlighted(gameObject, true);
        }
    }

    public void Unhighlight()
    {
        isHighlighted = false;
        button.colors = ColorBlock.defaultColorBlock;
    }

    public void MarkImportant()
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
