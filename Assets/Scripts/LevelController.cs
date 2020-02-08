using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public GameObject mapUI;
    InputFreeLookCam cam;
    PlayerMovement playerMove;

    public void Start()
    {
        mapUI.SetActive(false);

        cam = FindObjectOfType<InputFreeLookCam>();
        playerMove = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("MainMenu");
    }

    public void GoToAbbysApartment()
    {
        SceneManager.LoadScene("Abby_Apartment_Test");
        CloseMap();
    }

    public void GoToTristansApartment()
    {
        SceneManager.LoadScene("Tristan_Apartment_Test");
        CloseMap();
    }

    public void GoToBernardsApartment()
    {
        SceneManager.LoadScene("Bernard_Apartment_Test");
        CloseMap();
    }

    public void ManagementOffice()
    {
        SceneManager.LoadScene("Management_Office_Test");
        CloseMap();
    }

    public void OwnersSuite()
    {
        SceneManager.LoadScene("Owner_Suite_Test");
        CloseMap();
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
