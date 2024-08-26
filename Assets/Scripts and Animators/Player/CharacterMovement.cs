using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed = 6f; //movement speed
    public float jumpSpeed = 8f;
    public float gravity = -5f;
    public float terminalVelocity = -20f;
    public float vertSpeed;
    private Animator animator;
    private CharacterController controller;
    private Camera cam;
    [SerializeField] private AudioSource footstepsSound;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = GetComponent<Camera>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        Vector3 moveDirection = Vector3.zero;

        //get input for horizontal and vertical movement
        float vInput = Input.GetAxis("Vertical");
        float hInput = Input.GetAxis("Horizontal");
        moveDirection = new Vector3(hInput, 0, vInput);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;

        //set animator triggers based on movement input
        if (vInput > 0 || hInput != 0)
        {
            animator.SetTrigger("Forward");
            footstepsSound.enabled = true;
        }
        else if (vInput < 0)
        {
            animator.SetTrigger("Backward");
            footstepsSound.enabled = true;
        }
        else
        {
            animator.SetTrigger("Idle");
            footstepsSound.enabled = false;
        }

        if (controller.isGrounded)
        {
            //check for jump input
            if (Input.GetButtonDown("Jump") && controller.isGrounded)
            {
                vertSpeed = jumpSpeed;
                animator.SetTrigger("Jump");
            }
        }
        else if (!controller.isGrounded)
        {
            vertSpeed += gravity * 5 * Time.deltaTime;
            if (vertSpeed < terminalVelocity)
            {
                vertSpeed = terminalVelocity;
            }
        }
        moveDirection.y = vertSpeed;

        //move the character controller
        controller.Move(moveDirection * Time.deltaTime);

        // Reset the triggers to prevent repeated triggering
        if (controller.isGrounded)
        {
            if (vInput == 0 && hInput == 0)
            {
                animator.ResetTrigger("Forward");
                animator.ResetTrigger("Backward");
            }
            animator.ResetTrigger("Jump");
        }
    }
}
