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
    public bool movePlayer;
    public float rotationSmoothing;

    public GameObject npc;

    public GameObject gameCam;
    public GameObject cutsceneCam;
    public GameObject dialogueCam;
    public GameObject questioningCam;

    bool isDoneTalking = true;
    bool isQuestioning = false;

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
        fadeImage.enabled = true;
        fadeImage.canvasRenderer.SetAlpha(0f);
    }

    public void SwitchToDialogueCam()
    {
        isQuestioning = false;

        gameCam.SetActive(false);
        cutsceneCam.SetActive(false);
        dialogueCam.SetActive(true);
        questioningCam.SetActive(false);

        npc.transform.rotation = Quaternion.Lerp(npc.transform.rotation, Quaternion.Euler(npc.transform.rotation.x, npc.transform.rotation.y + npcRotationOffset, npc.transform.rotation.z), rotationSmoothing  * Time.deltaTime);
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

        npc.transform.rotation = Quaternion.Lerp(npc.transform.rotation, Quaternion.Euler(npc.transform.rotation.x, npc.transform.rotation.y - npcRotationOffset, npc.transform.rotation.z), rotationSmoothing  * Time.deltaTime);
    }

    public void StartFade(bool isDoneTalking)
    {
        playerMovement.StopMovement();
        playerAnimController.ChangePlayerAnim(1);
        playerMovement.moveDirection = Vector3.zero;
        StartCoroutine("Fade", isDoneTalking);
    }

    public void DoneTalking()
    {
        isDoneTalking = !isDoneTalking;
    }

    IEnumerator Fade(bool isDoneTalking)
    {
        // fade in
        fadeImage.CrossFadeAlpha(1, fadeIn, false);

        yield return new WaitForSeconds(fadeIn);

        // fade transition- change cameras when screen is black
        if (!isDoneTalking)
            SwitchToDialogueCam();
        else
            SwitchToGameCam();

        transform.position = target.position;
        transform.rotation = target.rotation;

        yield return new WaitForSeconds(fadeTranstition);

        // fade out
        fadeImage.CrossFadeAlpha(0, fadeOut, false);

        yield return new WaitForSeconds(fadeOut);

        if (isDoneTalking)
            playerMovement.ResumeMovement();
    }
}

