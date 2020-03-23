using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMap : MonoBehaviour
{
    public GameObject mapUI;
    PlayerMovement playerMove;
    InputFreeLookCam cam;
    Journal journal;

    private void Start()
    {
        playerMove = FindObjectOfType<PlayerMovement>();
        cam = FindObjectOfType<InputFreeLookCam>();
        journal = FindObjectOfType<Journal>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            mapUI.SetActive(true);

            playerMove.StopMovement();

            cam.FreezeCamera();

            journal.CloseJournals();
            journal.CanOpenJournals(false);
        }
    }
}
