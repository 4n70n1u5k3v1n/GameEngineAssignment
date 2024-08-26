using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    public Camera playerCamera;       //the main camera
    public Camera overheadCamera;     //the overhead camera for the puzzle view
    public KeyCode switchKey = KeyCode.P;  //the key to switch between cameras
    private bool isPlayerInTrigger = false;

    void Start()
    {
        //ensure that the overhead camera is disabled at the start
        overheadCamera.gameObject.SetActive(false);
        playerCamera = Camera.main;
        playerCamera.gameObject.SetActive(true);
        LockCursor(true); //lock the cursor initially
    }

    void Update()
    {
        //check if the player is in the trigger area and the switch key is pressed
        if (isPlayerInTrigger && Input.GetKeyDown(switchKey))
        {
            if (playerCamera.gameObject.activeSelf)
            {
                Debug.Log("Switching to overhead camera.");
                //switch to the overhead camera
                playerCamera.gameObject.SetActive(false);
                overheadCamera.gameObject.SetActive(true);
                LockCursor(false); //unlock the cursor
            }
            else
            {
                Debug.Log("Switching to player camera.");
                //switch back to the player camera
                overheadCamera.gameObject.SetActive(false);
                playerCamera.gameObject.SetActive(true);
                LockCursor(true); //lock the cursor
            }
        }
    }

    //detect when the player enters the trigger area
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) //assuming the player has the "Player" tag
        {
            isPlayerInTrigger = true;
            Debug.Log("Player entered the puzzle area.");
        }
    }

    //detect when the player leaves the trigger area
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            Debug.Log("Player left the puzzle area.");
            //ensure the player camera is active and the overhead camera is inactive
            overheadCamera.gameObject.SetActive(false);
            playerCamera.gameObject.SetActive(true);
            LockCursor(true); //lock the cursor
        }
    }

    //method to lock or unlock the cursor
    private void LockCursor(bool lockCursor)
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
