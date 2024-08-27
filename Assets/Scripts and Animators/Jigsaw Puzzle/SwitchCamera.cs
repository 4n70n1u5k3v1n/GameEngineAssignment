using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    public Camera playerCamera;       // The main camera
    public Camera overheadCamera;     // The overhead camera for the puzzle view
    public KeyCode switchKey = KeyCode.P;  // The key to switch between cameras
    private bool isPlayerInTrigger = false;

    public GameObject cheatButton; // Reference to the cheat button
    public GameObject cheatText; // Reference to the cheat text
    public GameObject resetButton; // Reference to the reset button
    public GameObject resetText; // Reference to the reset text
    public GameObject puzzleFrame; // Reference to the puzzle frame
    public GameObject puzzleImage; // Reference to the puzzle image

    void Start()
    {
        // Ensure that the overhead camera is disabled at the start
        overheadCamera.gameObject.SetActive(false);
        playerCamera = Camera.main;
        playerCamera.gameObject.SetActive(true);
        LockCursor(true); // Lock the cursor initially

        // Ensure buttons are initially disabled
        SetObjectsVisibility(false);
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

                // Enable buttons
                SetObjectsVisibility(true);
            }
            else
            {
                Debug.Log("Switching to player camera.");
                // Switch back to the player camera
                overheadCamera.gameObject.SetActive(false);
                playerCamera.gameObject.SetActive(true);
                LockCursor(true); // Lock the cursor

                // Disable buttons
                SetObjectsVisibility(false);
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

            // Disable buttons
            SetObjectsVisibility(false);
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

    // Method to set the visibility of the buttons
    private void SetObjectsVisibility(bool isVisible)
    {
        cheatButton.GetComponent<MeshRenderer>().enabled = isVisible;
        resetButton.GetComponent<MeshRenderer>().enabled = isVisible;
        puzzleFrame.GetComponent<MeshRenderer>().enabled = isVisible;

        // Enable/Disable TextMeshPro components
        cheatText.GetComponent<MeshRenderer>().enabled = isVisible;
        resetText.GetComponent<MeshRenderer>().enabled = isVisible;

        // Enable / Disable sprite renderer
        puzzleImage.GetComponent<SpriteRenderer>().enabled = isVisible;
    }
}
