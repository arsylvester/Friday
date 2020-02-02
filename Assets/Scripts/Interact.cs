using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Yarn.Unity.Example;
using UnityEngine.Events;

public class Interact : MonoBehaviour
{
    private DialogueRunner dialogue;
    public float range;

    MovePlayerToLocation player;

    private void Start()
    {
        dialogue = FindObjectOfType<DialogueRunner>();
        player = FindObjectOfType<MovePlayerToLocation>();
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0) && !dialogue.isDialogueRunning && Physics.Raycast(ray, out hit, range))
        {
            print("Interacting with: " + hit);
            var target = hit.transform.GetComponent<NPC>();
            if (target != null)
            {
                dialogue.StartDialogue(target.talkToNode);
                player.MovePlayer();
            }
        }
    }
}
