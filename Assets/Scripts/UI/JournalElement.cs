using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class JournalElement : MonoBehaviour
{
    public string keyID;
    public bool isHighlighted;
    public bool isMarked = false;
    public ColorBlock highlightedColors = ColorBlock.defaultColorBlock;
    public Transform journalParent;

    public abstract void Clicked();

    public abstract void Highlight();

    public abstract void Unhighlight();

    public abstract void MarkImportant();
    public abstract void DeleteEntry();
}
