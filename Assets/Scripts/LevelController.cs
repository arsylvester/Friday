using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Yarn.Unity;

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

    static bool abbyOpen = true;
    static bool tristanOpen = false;
    static bool bernardOpen = false;
    static bool managementOpen = false;
    bool ownerOpen = false;

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
            DisableEnableJournalForPause(isPaused);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            ResumeGame();
            DisableEnableJournalForPause(isPaused);
        }
    }

    void DisableEnableJournalForPause(bool isPaused)
    {
        Journal journal = FindObjectOfType<Journal>();
        if (isPaused)
            journal.enabled = false;
        else
            journal.enabled = true;

        GameObject itemJournal = GameObject.Find("Item Journal");

        if (itemJournal != null)
        {
            GameObject itemCatalog = itemJournal.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
            Button[] items = itemCatalog.GetComponentsInChildren<Button>();

            foreach (Button b in items)
            {
                if (isPaused)
                    b.interactable = false;
                else
                    b.interactable = true;
            }
        }

        GameObject dialogueJournal = GameObject.Find("Dialogue Journal");

        if (dialogueJournal != null)
        {
            GameObject dialogueCatalog = dialogueJournal.transform.GetChild(0).GetChild(0).gameObject;
            Button[] items = dialogueCatalog.GetComponentsInChildren<Button>();

            foreach (Button b in items)
            {
                if (isPaused)
                    b.interactable = false;
                else
                    b.interactable = true;
            }
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseUI.SetActive(false);
        isPaused = false;

        interact.enabled = true;
        DisableEnableJournalForPause(isPaused);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");

        isPaused = false;
        DisableEnableJournalForPause(isPaused);
    }

    public void GoToAbbysApartment()
    {
        if(abbyOpen)
        {
            location = "Abby_Room";
            confirmLocationText.text = "Abby's Apartment?";
            confirmLocationUI.SetActive(true);
        }
    }

    public void GoToTristansApartment()
    {
        if(tristanOpen)
        {
            location = "Tristan_Room";
            confirmLocationText.text = "Tristan's Apartment?";
            confirmLocationUI.SetActive(true);
        }
    }

    public void GoToBernardsApartment()
    {
        if(bernardOpen)
        {
            location = "Bernard_Room";
            confirmLocationText.text = "Bernard's Apartment?";
            confirmLocationUI.SetActive(true);
        }
    }

    public void ManagementOffice()
    {
        if(managementOpen)
        {
            location = "Management_Office";
            confirmLocationText.text = "Management Office?";
            confirmLocationUI.SetActive(true);
        }
    }

    public void OwnersSuite()
    {
        if(ownerOpen)
        {
            location = "Owner_Suite_Test";
            confirmLocationText.text = "Owner's Suite?";
            confirmLocationUI.SetActive(true);
        }
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

    public void CloseMap()
    {
        mapUI.SetActive(false);

        playerMove.ResumeMovement();

        cam.UnfreezeCamera();
    }

    [YarnCommand("openlocation")]
    public void OpenLocation(string sceneName)
    {
        switch(sceneName)
        {
            case "Abby_Room":
                abbyOpen = true;
                break;
            case "Tristan_Room":
                tristanOpen = true;
                break;
            case "Bernard_Room":
                bernardOpen = true;
                break;
            case "Management_Office":
                managementOpen = true;
                break;
            case "Owner_Suite_Test":
                abbyOpen = true;
                break;
            default:
                break;
        }
    }
}
