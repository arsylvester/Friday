﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using Yarn.Unity.Example;

public class Interact : MonoBehaviour
{
    private DialogueRunner dialogue;

    private void Start()
    {
        dialogue = FindObjectOfType<DialogueRunner>();
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0) && !dialogue.isDialogueRunning && Physics.Raycast(ray, out hit, 1000))
        {
            print("Interacting with: " + hit);
            var target = hit.transform.GetComponent<NPC>();
            if (target != null)
                dialogue.StartDialogue(target.talkToNode);
        }
    }
}