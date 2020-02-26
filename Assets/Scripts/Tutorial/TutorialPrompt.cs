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
    
    // Start is called before the first frame update
    void Start()
    {
        //Vector3 temp = fadePanel.transform.position;
        //temp.z = fadeDepth;
        //fadePanel.transform.position = temp;
    }

    public void StartPrompt(GameObject highlightedObject, GameObject prompts)
    {
        highlighted = highlightedObject;
        currentPrompts = prompts;
        //orgDepth = highlightedObject.position.z;

        //Vector3 temp = highlightedObject.position;
        //temp.z = fadeDepth + 5;
        //highlightedObject.position = temp;

        prompts.SetActive(true);
        fadePanel.SetActive(true);

        Instantiate(highlightedObject, transform, true);
    }

    public void EndPrompt()
    {
        //Vector3 temp = highlighted.position;
        //temp.z = orgDepth;
        //highlighted.position = temp;

        currentPrompts.SetActive(false);
        fadePanel.SetActive(false);

        Destroy(highlighted);

        TutorialState.Next();
    }
}
