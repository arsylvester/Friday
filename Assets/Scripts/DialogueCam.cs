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

    public GameObject gameCam;
    public GameObject cutsceneCam;
    public GameObject dialogueCam;
    public GameObject questioningCam;

    bool isQuestioning = false;

    private void Start()
    {
        charController = GetComponent<CharacterController>();

        gameCam.SetActive(true);
        cutsceneCam.SetActive(false);
        dialogueCam.SetActive(false);
        questioningCam.SetActive(false);
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
        }
        else
        {
            if (!isQuestioning)
                SwitchToDialogueCam();
            else
                SwitchToQuestioningCam();

            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rotationSmoothing);
            npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.Euler(npc.transform.rotation.x, npc.transform.rotation.y - 200, npc.transform.rotation.z), rotationSmoothing);
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

