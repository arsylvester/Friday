using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DialogueCam : MonoBehaviour
{
    public Transform target;
    public float speed;

    CharacterController charController;
    public bool movePlayer;
    public float rotationSmoothing;

    public GameObject npc;
    public float npcRotationOffset;
    public float npcOrigRotationOffset;

    public GameObject gameCam;
    public GameObject cutsceneCam;
    public GameObject dialogueCam;
    public GameObject questioningCam;

    bool isQuestioning = false;

    PlayerAnimController playerAnimController;

    private void Start()
    {
        charController = GetComponent<CharacterController>();

        gameCam.SetActive(true);
        cutsceneCam.SetActive(false);
        dialogueCam.SetActive(false);
        questioningCam.SetActive(false);

        playerAnimController = FindObjectOfType<PlayerAnimController>();
    }

    void Update()
    {
        if (movePlayer)
        {
            MoveToLocation();
        }
    }

    public void MoveToLocation()
    {
        Vector3 dir = target.position - transform.position;
        Vector3 movement = dir.normalized * speed * Time.deltaTime;

        // limit movement to never pass the target position
        if (movement.magnitude > dir.magnitude) movement = dir;

        charController.Move(movement);
        Rotation(movement);
    }

    void Rotation(Vector3 movement)
    {
        var moveDir = movement;
        moveDir.y = 0;

        if (Vector3.Distance(transform.position, target.position) > 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), rotationSmoothing);
            playerAnimController.SetAnimator(2);
        }
        else
        {
            if (!isQuestioning)
                SwitchToDialogueCam();
            else
                SwitchToQuestioningCam();

            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rotationSmoothing);

            playerAnimController.SetAnimator(1);
        }
    }

    public void SwitchToCutsceneCam()
    {
        gameCam.SetActive(false);
        cutsceneCam.SetActive(true);
        dialogueCam.SetActive(false);
        questioningCam.SetActive(false);
    }

    public void SwitchToDialogueCam()
    {
        isQuestioning = false;

        gameCam.SetActive(false);
        cutsceneCam.SetActive(false);
        dialogueCam.SetActive(true);
        questioningCam.SetActive(false);

        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.Euler(npc.transform.rotation.x, npc.transform.rotation.y + npcRotationOffset, npc.transform.rotation.z), rotationSmoothing);
    }

    public void SwitchToQuestioningCam()
    {
        isQuestioning = true;

        gameCam.SetActive(false);
        cutsceneCam.SetActive(false);
        dialogueCam.SetActive(false);
        questioningCam.SetActive(true);
    }

    public void SwitchToGameCam()
    {
        gameCam.SetActive(true);
        cutsceneCam.SetActive(false);
        dialogueCam.SetActive(false);
        questioningCam.SetActive(false);

        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.Euler(npc.transform.rotation.x, npc.transform.rotation.y + npcOrigRotationOffset, npc.transform.rotation.z), rotationSmoothing);
    }

    public void MovePlayer()
    {
        movePlayer = true;
    }

    public void StopMovingPlayer()
    {
        movePlayer = false;
    }
}

