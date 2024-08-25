using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    public Camera playerCamera;       // The main camera, typically the one following the player
    public Camera overheadCamera;     // The overhead camera for the puzzle view
    public KeyCode switchKey = KeyCode.P;  // The key to switch between cameras
    //public DragAndDrop_ dragAndDropScript; // Reference to the DragAndDrop_ script
    private bool isPlayerInTrigger = false;

    void Start()
    {
        // Ensure that the overhead camera is disabled at the start
        overheadCamera.gameObject.SetActive(false);
        playerCamera = Camera.main; // Use the main camera from DragAndDrop_
        playerCamera.gameObject.SetActive(true);
        LockCursor(true); // Lock the cursor initially
    }

    void Update()
    {
        // Check if the player is in the trigger area and the switch key is pressed
        if (isPlayerInTrigger && Input.GetKeyDown(switchKey))
        {
            if (playerCamera.gameObject.activeSelf)
            {
                Debug.Log("Switching to overhead camera.");
                // Switch to the overhead camera
                playerCamera.gameObject.SetActive(false);
                overheadCamera.gameObject.SetActive(true);
                LockCursor(false); // Unlock the cursor
            }
            else
            {
                Debug.Log("Switching to player camera.");
                // Switch back to the player camera
                overheadCamera.gameObject.SetActive(false);
                playerCamera.gameObject.SetActive(true);
                LockCursor(true); // Lock the cursor
            }
        }
    }

    // Detect when the player enters the trigger area
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming the player has the "Player" tag
        {
            isPlayerInTrigger = true;
            Debug.Log("Player entered the puzzle area.");
        }
    }

    // Detect when the player leaves the trigger area
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            Debug.Log("Player left the puzzle area.");
            // Ensure the player camera is active and the overhead camera is inactive
            overheadCamera.gameObject.SetActive(false);
            playerCamera.gameObject.SetActive(true);
            LockCursor(true); // Lock the cursor
        }
    }

    // Method to lock or unlock the cursor
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
