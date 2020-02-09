using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public GameObject pauseUI;
    public GameObject mapUI;
    InputFreeLookCam cam;

    public void Start()
    {
        pauseUI.SetActive(false);
        mapUI.SetActive(false);

        cam = FindObjectOfType<InputFreeLookCam>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            pauseUI.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseUI.SetActive(false);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToAbbysApartment()
    {
        SceneManager.LoadScene("Abby_Room");
    }

    public void GoToTristansApartment()
    {
        SceneManager.LoadScene("Tristan_Apartment");
    }

    public void GoToBernardsApartment()
    {
        SceneManager.LoadScene("Bernard_Apartment");
    }

    public void ManagementOffice()
    {
        SceneManager.LoadScene("Management_Office");
    }

    public void OwnersSuite()
    {
        SceneManager.LoadScene("Owner_Suite");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            mapUI.SetActive(true);

            PlayerMovement p = other.GetComponent<PlayerMovement>();
            p.StopMovement();

            cam.FreezeCamera();
        }
    }
}
