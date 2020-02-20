using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractOnStart : MonoBehaviour
{
    [SerializeField] PlayerInteractionAgent agent; 
    [SerializeField] Interactable NPC;
    [SerializeField] float waitTime = 1f;

    private static bool isFirstTime = true; //Probably want to remove in the future.

    // Start is called before the first frame update
    void Start()
    {
        if(isFirstTime)
            StartCoroutine(StartDialogue());
    }

    IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(waitTime);
        agent.Interact(NPC);
        isFirstTime = false;
    }

}
