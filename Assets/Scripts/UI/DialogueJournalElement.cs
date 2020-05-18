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
    [SerializeField] float smallRectHeight = 100;
    [SerializeField] float smallRectWidth = 150;
    [SerializeField] GameObject markImportantButton;
    [SerializeField] GameObject deleteButton;

    private ColorBlock unMarkedColors;

    private Button button;
    private Journal journal;
    private Text text;
    private RectTransform rectTransform;
    private float rectHeight;
    private float rectWidth;
    private float textWidth;
    private float textHeight;
   // private bool isMarked = false;

    private void Awake()
    {
        button = GetComponent<Button>();
        journal = GetComponentInParent<Journal>();
        text = GetComponentInChildren<Text>();
        rectTransform = GetComponent<RectTransform>();

        rectHeight = rectTransform.sizeDelta.y;
        rectWidth = rectTransform.sizeDelta.x;
        textWidth = textRect.sizeDelta.x;
        textHeight = textRect.sizeDelta.y;
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
        unMarkedColors = button.colors;
        button.colors = highlightedColors;
        if (buttons)
        {
            markImportantButton.SetActive(true);
            deleteButton.SetActive(true);
        }
    }

    public override void Unhighlight()
    {
        isHighlighted = false;
        button.colors = unMarkedColors;
        ResizeRegular();
        markImportantButton.SetActive(false);
        deleteButton.SetActive(false);
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

    public void ResizeSmall()
    {
        rectTransform.sizeDelta = new Vector2(smallRectWidth, smallRectHeight);
        textRect.sizeDelta = new Vector2(smallRectWidth, smallRectHeight);
        textRect.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
    }

    public void ResizeRegular()
    {
        rectTransform.sizeDelta = new Vector2(rectWidth, rectHeight);
        textRect.sizeDelta = new Vector2(textWidth, textHeight);
        textRect.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
    }

    public override void DeleteEntry()
    {
        journal.ConfirmDeletion();
    }
}
