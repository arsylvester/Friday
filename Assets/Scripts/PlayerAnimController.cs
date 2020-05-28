using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    public enum NPCState { none, idle, moving, takeOutNotebook, idleWithNotebook, writeInNotebook }
    public NPCState npcState;

    public Animator animator;

    PlayerMovement playerMovement;

    DialogueCam dialogueCam;
    LevelController levelController;

    void Start()
    {
        animator = GetComponent<Animator>();

        playerMovement = GetComponent<PlayerMovement>();

        dialogueCam = GetComponent<DialogueCam>();
        levelController = FindObjectOfType<LevelController>();
    }

    private void Update()
    {
        if (playerMovement.moveDirection.x == 0 && playerMovement.moveDirection.z == 0 && dialogueCam.isDoneTalking && !levelController.mapMoveBack)
            animator.SetInteger("MainCharAnim", (int)NPCState.idle);
        else if (dialogueCam.isDoneTalking && animator.GetInteger("MainCharAnim") != 5)
            animator.SetInteger("MainCharAnim", (int)NPCState.moving);
    }

    public void ChangePlayerAnim(int anim)
    {
        animator.SetInteger("MainCharAnim", anim);

        if (anim == 4)
        {
            animator.SetBool("writing", true);
        }
    }

    public void StopWritingAnim()
    {
        animator.SetBool("writing", false);
    }
}
