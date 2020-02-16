using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerMovement : MonoBehaviour
{
    public bool isStopped;

    public float moveSpeed;
    public float sprintSpeed;
    public float rotationSmoothing;

    public Transform cameraTransform;

    CharacterController charController;
    public Vector3 moveDirection;
    float gravity = 1000;
    float tmpSpeed;

    PlayerAnimController playerAnimController;
    DialogueUI runner;

    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<CharacterController>();
        playerAnimController = GetComponent<PlayerAnimController>();

        tmpSpeed = moveSpeed;

        runner = FindObjectOfType<DialogueUI>();
        runner.onDialogueStart.AddListener(StopMovement);
        runner.onDialogueEnd.AddListener(ResumeMovement);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStopped)
        {
            Movement();
            Rotation();
        }
    }

    void Movement()
    {
        // get input to move the player while grounded
        if (charController.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            // have player move forward in the direction of the camera
            moveDirection = cameraTransform.TransformDirection(moveDirection);
            moveDirection *= moveSpeed;

            // sprint input
            if (Input.GetKey(KeyCode.LeftShift))
                moveSpeed = sprintSpeed;
            else
                moveSpeed = tmpSpeed;
        }

        // apply gravity in the case that the player isn't grounded 
        moveDirection.y -= gravity * Time.deltaTime;

        // move the player
        charController.Move(moveDirection * Time.deltaTime);
    }

    void Rotation()
    {
        // if the player is moving, rotate the player in the direction of movement
        if (moveDirection.x != 0 || moveDirection.z != 0)
        {
            var dir = moveDirection;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotationSmoothing);
        }
    }

    public void StopMovement()
    {
        isStopped = true;
        moveDirection = new Vector3(0, 0, 0);
        playerAnimController.animator.SetInteger("MainCharAnim", 1);
    }

    public void ResumeMovement()
    {
        isStopped = false;
    }
}
