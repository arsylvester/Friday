using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Yarn.Unity;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueCam : MonoBehaviour
{
    GameObject[] potentialTargets;
    GameObject target;
    float closestPlayerDistance;
    bool firstLocation = true;

    public float speed;

    CharacterController charController;
    public Quaternion tmpNpcRotation;

    public GameObject npc;

    public GameObject gameCam;

    GameObject[] potentialDialogueCams;
    float closestDialogueCam;
    bool firstDialogueCam = true;
    int index;

    GameObject[] potentialQuestioningCams;
    float closestQuestioningCam;
    bool firstQuestioningCam = true;

    public bool isDoneTalking = true;
    public bool isQuestioning = false;

    PlayerAnimController playerAnimController;

    public float npcRotationOffset;

    private Journal journal;
    DialogueUI runner;

    PlayerMovement playerMovement;
    public GameObject fade;
    Image fadeImage;
    public float fadeIn;
    public float fadeTranstition;
    public float fadeOut;

    private void Start()
    {
        charController = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();

        gameCam.SetActive(true);

        playerAnimController = FindObjectOfType<PlayerAnimController>();

        journal = FindObjectOfType<Journal>();
        journal.OnQuestionStart.AddListener(SwitchToQuestioningCam);
        journal.OnQuestionStop.AddListener(SwitchToDialogueCam);

        runner = FindObjectOfType<DialogueUI>();
        runner.onDialogueStart.AddListener(DoneTalking);
        runner.onDialogueStart.AddListener(delegate{StartFade(isDoneTalking);});
        runner.onDialogueEnd.AddListener(DoneTalking);
        runner.onDialogueEnd.AddListener(delegate { StartFade(isDoneTalking); });

        fadeImage = fade.GetComponent<Image>();

        tmpNpcRotation = npc.transform.rotation;
    }

    public void SwitchToDialogueCam()
    {
        isQuestioning = false;

        gameCam.SetActive(false);

        potentialDialogueCams = GameObject.FindGameObjectsWithTag("DialogueCam");
        potentialQuestioningCams = GameObject.FindGameObjectsWithTag("QuestioningCam");

        // find dialogue cam to turn on
        for (int i = 0; i < potentialDialogueCams.Length; i++)
        {
            float distance = Vector3.Distance(potentialTargets[i].transform.position, playerMovement.transform.position);
            if (firstDialogueCam)
            {
                firstDialogueCam = false;

                closestDialogueCam = distance;
                index = i;
            }
            else if (distance < closestDialogueCam)
            {
                closestDialogueCam = distance;
                index = i;
            }
        }

        // turn on/off all dialogue cams
        for (int i = 0; i < potentialDialogueCams.Length; i++)
        {
            if (i == index)
            {
                potentialDialogueCams[i].GetComponent<CinemachineVirtualCamera>().Priority = 10;
            }
            else
                potentialDialogueCams[i].GetComponent<CinemachineVirtualCamera>().Priority = 9;
        }

        // reset questioning cams
        for (int i = 0; i < potentialDialogueCams.Length; i++)
        {
            if (i == index)
            {
                potentialQuestioningCams[i].GetComponent<CinemachineVirtualCamera>().Priority = 9;
            }
            else
                potentialQuestioningCams[i].GetComponent<CinemachineVirtualCamera>().Priority = 9;
        }
        // playerAnimController.ChangePlayerAnim(3);
    }

    public void SwitchToQuestioningCam()
    {
        isQuestioning = true;

        gameCam.SetActive(false);

        potentialDialogueCams = GameObject.FindGameObjectsWithTag("DialogueCam");
        potentialQuestioningCams = GameObject.FindGameObjectsWithTag("QuestioningCam");

        // find questioning cam to turn on
        for (int i = 0; i < potentialDialogueCams.Length; i++)
        {
            float distance = Vector3.Distance(potentialTargets[i].transform.position, playerMovement.transform.position);
            if (firstQuestioningCam)
            {
                firstQuestioningCam = false;

                closestQuestioningCam = distance;
                index = i;
            }
            else if (distance < closestQuestioningCam)
            {
                closestQuestioningCam = distance;
                index = i;
            }
        }
        // turn on/off all questioning cams
        for (int i = 0; i < potentialDialogueCams.Length; i++)
        {
            if (i == index)
                potentialQuestioningCams[i].GetComponent<CinemachineVirtualCamera>().Priority = 10;
            else
                potentialDialogueCams[i].GetComponent<CinemachineVirtualCamera>().Priority = 9;
        }
        // reset dialogue cams
        for (int i = 0; i < potentialDialogueCams.Length; i++)
        {
            if (i == index)
                potentialDialogueCams[i].GetComponent<CinemachineVirtualCamera>().Priority = 9;
            else
                potentialDialogueCams[i].GetComponent<CinemachineVirtualCamera>().Priority = 9;
        }
    }

    public void SwitchToGameCam()
    {
        gameCam.SetActive(true);
 
        for (int i = 0; i < potentialDialogueCams.Length; i++)
        {
            potentialDialogueCams[i].GetComponent<CinemachineVirtualCamera>().Priority = 9;
            potentialDialogueCams[i].GetComponent<CinemachineVirtualCamera>().Priority = 9;
        }
    }

    public void StartFade(bool isDoneTalking)
    {
        fadeImage.enabled = true;
        playerMovement.enabled = false;
        playerMovement.StopMovement();

        StartCoroutine("Fade", isDoneTalking);
    }

    public void DoneTalking()
    {
        isDoneTalking = !isDoneTalking;
    }

    IEnumerator Fade(bool isDoneTalking)
    {
        // find closest dialogue location
        potentialTargets = GameObject.FindGameObjectsWithTag("DialogueLocation");
        foreach (GameObject obj in potentialTargets)
        {
            float distance = Vector3.Distance(obj.transform.position, playerMovement.transform.position);
            if (firstLocation)
            {
                firstLocation = false;

                target = obj;
                closestPlayerDistance = distance;
            }
            else if (distance < closestPlayerDistance)
            {
                target = obj;
                closestPlayerDistance = distance;
            }
        }

        fadeImage.canvasRenderer.SetAlpha(0f);

        // fade in
        fadeImage.CrossFadeAlpha(1, fadeIn, false);

        yield return new WaitForSeconds(fadeIn);

        // fade transition- change cameras when screen is black
        if (!isDoneTalking)
        {
            SwitchToDialogueCam();
            npc.transform.rotation = Quaternion.Euler(0, npcRotationOffset, 0);
        }
        else
        {
            SwitchToGameCam();
            npc.transform.rotation = tmpNpcRotation;
        }

        transform.position = target.transform.position;
        transform.rotation = target.transform.rotation;

        yield return new WaitForSeconds(fadeTranstition);

        // fade out
        fadeImage.CrossFadeAlpha(0, fadeOut, false);

        if (!isDoneTalking)
            playerAnimController.ChangePlayerAnim(3);
        else
            playerAnimController.ChangePlayerAnim(5);

        yield return new WaitForSeconds(fadeOut);

        fadeImage.enabled = false;

        if (isDoneTalking && (TutorialState.Current == "deduction" || TutorialState.Current == "done"))
        {
            yield return new WaitForSeconds(1.5f);
            playerMovement.enabled = true;
            playerMovement.ResumeMovement();
        }
    }
}

