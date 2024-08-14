using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SojaExiles

{
    public class PlayerMovement : MonoBehaviour
    {

        public CharacterController controller;

        public float speed = 5f;
        public float gravity = -15f;
        public float jumpSpeed = 15f;
        public float terminalVelocity = -20f;
        public float vertSpeed;

        Vector3 velocity;

        bool isGrounded;

        // Update is called once per frame
        void Update()
        {

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);

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

            move.y = vertSpeed;

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }
    }
}