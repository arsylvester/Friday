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

    public GameObject[] camerasToTurnOff;
    public GameObject cutscene3rdPersonCam;
    public GameObject cutscene1stPersonCam;

    private void Start()
    {
        charController = GetComponent<CharacterController>();

        foreach (GameObject cam in camerasToTurnOff)
        {
            cam.SetActive(true);
        }
        cutscene3rdPersonCam.SetActive(false);
        cutscene1stPersonCam.SetActive(false);
    }

    void Update()
    {
        if (movePlayer)
        {
            MoveToLocation();
        }

        // testing
        if (Input.GetKey(KeyCode.Alpha1))
            SwitchToDialogueCutsceneCam();

        if (Input.GetKey(KeyCode.Alpha2))
            SwitchToQuestioningCutsceneCam();

        if (Input.GetKey(KeyCode.Alpha3))
            SwitchToGameCam();
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

        if (movement != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), rotationSmoothing);
        else
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.parent.position), rotationSmoothing);
    }

    public void SwitchToDialogueCutsceneCam()
    {
        foreach (GameObject cam in camerasToTurnOff)
        {
            cam.SetActive(false);
        }
        cutscene3rdPersonCam.SetActive(true);
    }

    public void SwitchToQuestioningCutsceneCam()
    {
        cutscene3rdPersonCam.SetActive(false);
        cutscene1stPersonCam.SetActive(true);
    }

    public void SwitchToGameCam()
    {
        foreach (GameObject cam in camerasToTurnOff)
        {
            cam.SetActive(true);
        }
        cutscene3rdPersonCam.SetActive(false);
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

