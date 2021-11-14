using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Yarn.Unity.Example;
using UnityEngine.Events;

public class InteractWithObject : MonoBehaviour
{
    private DialogueRunner dialogue;
    public float range;

    // Start is called before the first frame update
    void Start()
    {
        dialogue = FindObjectOfType<DialogueRunner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0) && !dialogue.isDialogueRunning)
        {
            Debug.Log("clicking the mouse");
            var target = transform.gameObject.GetComponent<NPC>();
            if (target != null)
                dialogue.StartDialogue(target.talkToNode);
        }
    }
}
