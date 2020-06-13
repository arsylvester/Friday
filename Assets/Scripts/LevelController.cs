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
    public GameObject fadeImage;
    public GameObject confirmLocationUI;
    public TextMeshProUGUI confirmLocationText;
    public GameObject mapBackLocation;
    public bool mapMoveBack = false;
    LevelController levelController;

    public bool isPaused = false;
    string location;
    InputFreeLookCam cam;
    PlayerMovement playerMove;
    Journal journal;

    PlayerInteractionAgent interact;

    static bool abbyOpen = true;
    static bool tristanOpen = false;
    static bool bernardOpen = false;
    static bool managementOpen = false;
    bool ownerOpen = false;

    //TODO Make this visual cue for if a room is locked or not
    //cleaner
    public Button AbbyButton;
    public Button TristanButton;
    public Button BernardButton;
    public Button ManagementButton;
    public Button OwnerButton;
    //END

    public void Start()
    {
        pauseUI.SetActive(false);
        mapUI.SetActive(false);
        confirmLocationUI.SetActive(false);

        cam = FindObjectOfType<InputFreeLookCam>();
        playerMove = FindObjectOfType<PlayerMovement>();

        interact = FindObjectOfType<PlayerInteractionAgent>();

        journal = FindObjectOfType<Journal>();

        levelController = FindObjectOfType<LevelController>();
    }

    private void Update()
    {
        //TODO Make this visual cue for if a room is locked or not
        //cleaner
        AbbyButton.interactable = abbyOpen;
        TristanButton.interactable = tristanOpen;
        BernardButton.interactable = bernardOpen;
        ManagementButton.interactable = managementOpen;
        OwnerButton.interactable = ownerOpen;
        //END

        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused && (cam.gameObject.activeSelf && !mapUI.activeSelf && !fadeImage.GetComponent<Image>().enabled) && FindObjectOfType<InspectorAgent>().target == null)
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
        else if (Input.GetKeyDown(KeyCode.Escape) && !isPaused && mapUI.activeInHierarchy)
            levelController.CloseMap();

        if (mapMoveBack)
        {
            MoveToLocation();
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
            location = "Owner_Room";
            confirmLocationText.text = "Owner's Suite?";
            confirmLocationUI.SetActive(true);
        }
    }

    public void GoToLocation()
    {
        SceneManager.LoadScene(location);
        journal.CanOpenJournals(true);
    }

    public void ReturnToMap()
    {
        confirmLocationUI.SetActive(false);
    }

    public void CloseMap()
    {
        mapUI.SetActive(false);

        mapMoveBack = true;

        cam.UnfreezeCamera();

        journal.CanOpenJournals(true);
    }

    void MoveToLocation()
    {
        CharacterController player = FindObjectOfType<CharacterController>();
        PlayerAnimController playerAnim = player.GetComponent<PlayerAnimController>();

        Vector3 dir = mapBackLocation.transform.position - player.gameObject.transform.position;

        Vector3 movement = dir.normalized * player.gameObject.GetComponent<PlayerMovement>().moveSpeed * Time.deltaTime;

        // limit movement to never pass the target position
        if (movement.magnitude > dir.magnitude) movement = dir;

        player.Move(movement);
        playerAnim.ChangePlayerAnim(2);
        Rotation(player.gameObject, movement, playerAnim);
    }


    void Rotation(GameObject player, Vector3 movement, PlayerAnimController playerAnim)
    {
        var moveDir = movement;
        moveDir.y = 0;

        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(moveDir), 0.125f);

        if (Vector3.Distance(player.transform.position, mapBackLocation.transform.position) <= 0.25)
        {
            playerAnim.ChangePlayerAnim(1);
            playerMove.ResumeMovement();
            mapMoveBack = false;
        }
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
                ownerOpen = true;
                abbyOpen = false;
                tristanOpen = false;
                bernardOpen = false;
                managementOpen = false;
                break;
            default:
                break;
        }
    }
}
