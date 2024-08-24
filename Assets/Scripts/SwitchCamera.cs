using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    public Camera playerCamera;       // The main camera, typically the one following the player
    public Camera overheadCamera;     // The overhead camera for the puzzle view
    public KeyCode switchKey = KeyCode.P;  // The key to switch between cameras
    public DragAndDrop_ dragAndDropScript; // Reference to the DragAndDrop_ script
    private bool isPlayerInTrigger = false;

    private bool isOverheadView = false;

    void Start()
    {
        // Ensure that the overhead camera is disabled at the start
        overheadCamera.gameObject.SetActive(false);
        playerCamera = dragAndDropScript.mainCamera; // Use the main camera from DragAndDrop_
        playerCamera.gameObject.SetActive(true);
    }

    void Update()
    {
        // Check if the player is in the trigger area and the switch key is pressed
        if (isPlayerInTrigger && Input.GetKeyDown(switchKey))
        {
            // Switch to the overhead camera
            playerCamera.gameObject.SetActive(false);
            overheadCamera.gameObject.SetActive(true);
        }

        // If the player presses the key again, switch back to the player camera
        if (Input.GetKeyDown(switchKey) && overheadCamera.gameObject.activeSelf)
        {
            overheadCamera.gameObject.SetActive(false);
            playerCamera.gameObject.SetActive(true);
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
        }
    }
}
