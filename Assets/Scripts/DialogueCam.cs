using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Yarn.Unity;
using UnityEngine.UI;

public class DialogueCam : MonoBehaviour
{
    public Transform target;
    public float speed;

    CharacterController charController;
    public Quaternion tmpNpcRotation;

    public GameObject npc;

    public GameObject gameCam;
    public GameObject cutsceneCam;
    public GameObject dialogueCam;
    public GameObject questioningCam;

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
        cutsceneCam.SetActive(false);
        dialogueCam.SetActive(false);
        questioningCam.SetActive(false);

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
        cutsceneCam.SetActive(false);
        dialogueCam.SetActive(true);
        questioningCam.SetActive(false);

        playerAnimController.ChangePlayerAnim(3);
    }

    public void SwitchToQuestioningCam()
    {
        isQuestioning = true;

        gameCam.SetActive(false);
        cutsceneCam.SetActive(false);
        dialogueCam.SetActive(false);
        questioningCam.SetActive(true);

        playerAnimController.ChangePlayerAnim(4);
    }

    public void SwitchToGameCam()
    {
        gameCam.SetActive(true);
        cutsceneCam.SetActive(false);
        dialogueCam.SetActive(false);
        questioningCam.SetActive(false);
    }

    public void StartFade(bool isDoneTalking)
    {
        fadeImage.enabled = true;
        playerMovement.StopMovement();

        StartCoroutine("Fade", isDoneTalking);
    }

    public void DoneTalking()
    {
        isDoneTalking = !isDoneTalking;
    }

    IEnumerator Fade(bool isDoneTalking)
    {
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

        transform.position = target.position;
        transform.rotation = target.rotation;

        yield return new WaitForSeconds(fadeTranstition);

        // fade out
        fadeImage.CrossFadeAlpha(0, fadeOut, false);

        yield return new WaitForSeconds(fadeOut);

        fadeImage.enabled = false;

        if (isDoneTalking && TutorialState.Current == "deduction")
        {
            playerMovement.enabled = true;
            playerMovement.ResumeMovement();
        }
    }
}

