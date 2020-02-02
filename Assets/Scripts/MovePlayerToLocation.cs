using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MovePlayerToLocation : MonoBehaviour
{
    public Transform target;
    public float speed;

    bool movePlayer;

    public GameObject[] camerasToTurnOff;
    public GameObject cutsceneCam;

    private void Start()
    {

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
        if (transform.position != target.position)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);

            transform.LookAt(target.parent.transform);
        }
        else
        {
            movePlayer = false;
        }
    }

    public void SwitchToCutsceneCam()
    {
        foreach (GameObject cam in camerasToTurnOff)
        {
            cam.SetActive(false);
        }
        cutsceneCam.SetActive(true);
    }

    public void SwitchToGameCam()
    {
        foreach (GameObject cam in camerasToTurnOff)
        {
            cam.SetActive(true);
        }
        cutsceneCam.SetActive(false);
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

