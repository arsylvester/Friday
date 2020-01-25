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

    private Button button;
    private Journal journal;

    private void Awake()
    {
        button = GetComponent<Button>();
        journal = GetComponentInParent<Journal>();
    }

    public void Clicked()
    {
        isHighlighted = !isHighlighted;

        if(isHighlighted)
        {
            button.colors = highlightedColors;
            //journal.highlightedText.Add(gameObject);
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
