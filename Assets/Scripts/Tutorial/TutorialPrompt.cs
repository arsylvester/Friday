using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPrompt : MonoBehaviour
{
    [SerializeField] GameObject fadePanel;
    [SerializeField] float fadeDepth = 10;

    private GameObject highlighted;
    //private float orgDepth;
    private GameObject currentPrompts;
    private Transform highlightParent;
    private int sibIndex;
    private bool shouldHighlightReturn;
    private GameObject dupOfHighlight;
    
    // Start is called before the first frame update
    void Start()
    {
        //Vector3 temp = fadePanel.transform.position;
        //temp.z = fadeDepth;
        //fadePanel.transform.position = temp;
    }

    public void StartPrompt(GameObject highlightedObject, GameObject prompts, bool shouldReturn, GameObject dup)
    {
        //highlighted = Instantiate(highlightedObject, highlightedObject.transform.position, highlightedObject.transform.rotation, transform);
        //highlighted.GetComponent<Rect>().Set(highlightedObject.GetComponent<Rect>().x, highlightedObject.GetComponent<Rect>().y, highlightedObject.GetComponent<Rect>().width, highlightedObject.GetComponent<Rect>().height);
        highlighted = highlightedObject;
        currentPrompts = prompts;
        dupOfHighlight = dup;
        print(highlightedObject.transform.parent);
        highlightParent = highlightedObject.transform.parent;
        sibIndex = highlightedObject.transform.GetSiblingIndex();
        shouldHighlightReturn = shouldReturn;
        //orgDepth = highlightedObject.position.z;

        //Vector3 temp = highlightedObject.position;
        //temp.z = fadeDepth + 5;
        //highlightedObject.position = temp;

        prompts.SetActive(true);
        fadePanel.SetActive(true);

        highlighted.transform.SetParent(transform);
        //Instantiate(highlightedObject, transform, true);
    }

    public void EndPrompt()
    {
        //Vector3 temp = highlighted.position;
        //temp.z = orgDepth;
        //highlighted.position = temp;

        if(currentPrompts != null) currentPrompts.SetActive(false);
        if(fadePanel != null) fadePanel.SetActive(false);

        if(dupOfHighlight)
        {
            Destroy(dupOfHighlight);
        }

        //Destroy(highlighted);
        if(shouldHighlightReturn)
        {
            print("Setting back to: " + highlightParent);
            highlighted.transform.SetParent(highlightParent);
            highlighted.transform.SetSiblingIndex(sibIndex);
        }

        TutorialState.Next();
    }
}
