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

    bool isQuestioning = false;

    PlayerAnimController playerAnimController;

    public float npcRotationOffset;
    public float npcRotationOriginalOffset;

    private Journal journal;
    DialogueUI runner;

    public GameObject fade;
    Image fadeImage;
    public float fadeIn;
    public float fadeTransition;
    public float fadeOut;

    private void Start()
    {
        charController = GetComponent<CharacterController>();

        gameCam.SetActive(true);
        cutsceneCam.SetActive(false);
        dialogueCam.SetActive(false);
        questioningCam.SetActive(false);

        playerAnimController = FindObjectOfType<PlayerAnimController>();

        journal = FindObjectOfType<Journal>();
        journal.OnQuestionStart.AddListener(SwitchToQuestioningCam);
        journal.OnQuestionStop.AddListener(SwitchToDialogueCam);

        runner = FindObjectOfType<DialogueUI>();
        runner.onDialogueStart.AddListener(FadeIn);
        runner.onDialogueStart.AddListener(MovePlayer);
        runner.onDialogueEnd.AddListener(SwitchToGameCam);
        runner.onDialogueEnd.AddListener(StopMovingPlayer);

        fadeImage = fade.GetComponent<Image>();
        fadeImage.enabled = true;
        fadeImage.canvasRenderer.SetAlpha(0f);
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
        Vector3 dir = target.position - transform.position;
        Vector3 movement = dir.normalized * speed * 1.25f * Time.deltaTime;

        // limit movement to never pass the target position
        if (movement.magnitude > dir.magnitude) movement = dir;

        charController.Move(movement);
        Rotation(movement);
    }
    
    
    void Rotation(Vector3 movement)
    {
        var moveDir = movement;
        moveDir.y = 0;

        if (Vector3.Distance(transform.position, target.position) > 1)
        {
            playerAnimController.ChangePlayerAnim(2);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), rotationSmoothing);
        }
        else
        {
            if (!isQuestioning)
                SwitchToDialogueCam();
            else
                SwitchToQuestioningCam();

            playerAnimController.ChangePlayerAnim(1);
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rotationSmoothing);
            StartCoroutine("FadeOut", fadeTransition);
        }
    }
    
    public void FadeIn()
    {
        /*
        gameCam.SetActive(false);
        cutsceneCam.SetActive(true);
        dialogueCam.SetActive(false);
        questioningCam.SetActive(false);
        */
        fadeImage.CrossFadeAlpha(1, fadeIn, false);
    }

    IEnumerator FadeOut(int fadeTransition)
    {
        yield return new WaitForSeconds(fadeTransition);
        fadeImage.CrossFadeAlpha(0, fadeOut, false);
    }

    public void SwitchToDialogueCam()
    {
        isQuestioning = false;

        gameCam.SetActive(false);
        cutsceneCam.SetActive(false);
        dialogueCam.SetActive(true);
        questioningCam.SetActive(false);

        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.Euler(npc.transform.rotation.x, npc.transform.rotation.y + npcRotationOffset, npc.transform.rotation.z), rotationSmoothing);
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

        npc.transform.rotation = Quaternion.Slerp(npc.transform.rotation, Quaternion.Euler(npc.transform.rotation.x, npc.transform.rotation.y + npcRotationOriginalOffset, npc.transform.rotation.z), rotationSmoothing);
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

