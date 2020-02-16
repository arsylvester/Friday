﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelController : MonoBehaviour
{
    public GameObject pauseUI;
    public GameObject mapUI;
    public GameObject confirmLocationUI;
    public TextMeshProUGUI confirmLocationText;

    public bool isPaused = false;
    string location;
    InputFreeLookCam cam;
    PlayerMovement playerMove;

    PlayerInteractionAgent interact;

    public void Start()
    {
        pauseUI.SetActive(false);
        mapUI.SetActive(false);
        confirmLocationUI.SetActive(false);

        cam = FindObjectOfType<InputFreeLookCam>();
        playerMove = FindObjectOfType<PlayerMovement>();

        interact = FindObjectOfType<PlayerInteractionAgent>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            Time.timeScale = 0;
            pauseUI.SetActive(true);
            isPaused = true;

            interact.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseUI.SetActive(false);
        isPaused = false;

        interact.enabled = true;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToAbbysApartment()
    {
        location = "Abby_Room";
        confirmLocationText.text = "Abby's Apartment?";
        confirmLocationUI.SetActive(true);
    }

    public void GoToTristansApartment()
    {
        location = "Tristan_Room";
        confirmLocationText.text = "Tristan's Apartment?";
        confirmLocationUI.SetActive(true);
    }

    public void GoToBernardsApartment()
    {
        location = "Bernard_Room";
        confirmLocationText.text = "Bernard's Apartment?";
        confirmLocationUI.SetActive(true);
    }

    public void ManagementOffice()
    {
        location = "Management_Office";
        confirmLocationText.text = "Management Office?";
        confirmLocationUI.SetActive(true);
    }

    public void OwnersSuite()
    {
        location = "Owner_Suite_Test";
        confirmLocationText.text = "Owner's Suite?";
        confirmLocationUI.SetActive(true);
    }

    public void GoToLocation()
    {
        SceneManager.LoadScene(location);
    }

    public void ReturnToMap()
    {
        confirmLocationUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            mapUI.SetActive(true);

            playerMove.StopMovement();

            cam.FreezeCamera();
        }
    }

    private void CloseMap()
    {
        mapUI.SetActive(false);

        playerMove.ResumeMovement();

        cam.UnfreezeCamera();
    }
}
