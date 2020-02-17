using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class NPCAgent : MonoBehaviour
{
    private DialogueRunner dialogueRunner;
    void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }
    public void InitiateDialogue(Interactable interactable)
    {
        if(dialogueRunner == null)
            dialogueRunner = FindObjectOfType<DialogueRunner>();
        if(dialogueRunner != null)
        {
            InteractableNPC interactableNPC = interactable.GetComponent<InteractableNPC>();
            if (interactableNPC != null)
            {
                dialogueRunner.StartDialogue(interactableNPC.EntryNode);
            }
        }
        else
        {
            Debug.LogWarning("No dialogue runner found in scene.");
        }
    }
}
