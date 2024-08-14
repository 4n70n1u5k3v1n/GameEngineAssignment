using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement: MonoBehaviour
{
    public float speed = 6f; //movement speed
    public float jumpSpeed = 8f; 
    public float gravity = -5f; 
    public float terminalVelocity = -20f;
    public float vertSpeed;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 moveDirection = Vector3.zero;

        //get input for horizontal and vertical movement
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;

        //check for jump input
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            vertSpeed = jumpSpeed;
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
    }
}
