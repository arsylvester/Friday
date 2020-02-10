using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    public enum NPCState { none, idle, moving, talking }
    public NPCState npcState;

    private Animator animator;

    PlayerMovement playerMovement;

    void Start()
    {
        animator = GetComponent<Animator>();

        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        if (playerMovement.moveDirection.x == 0 && playerMovement.moveDirection.z == 0)
            animator.SetInteger("MainCharAnim", (int)NPCState.idle);
        else
            animator.SetInteger("MainCharAnim", (int)NPCState.moving);
    }
}
