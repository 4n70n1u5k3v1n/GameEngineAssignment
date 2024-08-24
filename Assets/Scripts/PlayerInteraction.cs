using SojaExiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // Variables needed to shoot rays
    private Camera playerCamera;
    public float interactRange = 5f; // The maximum range to interact with objects
    public LayerMask interactionLayerMask; // Layer mask to define what objects are interactable

    // Variable needed to pick up objects
    public Transform holdPosition; // The position where the object will be held
    private GameObject heldObject; // The object currently being held

    // Variable needed for battery machine and button
    public GameObject button; // The button that should turn green and become interactable
    public Material activeButtonMaterial; // Material to apply when the button becomes active
    private int batterySlotCount = 4; // Number of battery slots
    private int filledSlots = 0; // Number of filled battery slots
    [SerializeField] GameObject ovenDoor;
    [SerializeField] private Animator ovenAnimator;

    // Variables for note interaction
    private NoteScript activeNote;
    private GameObject interactMessage;

    void Start()
    {
        playerCamera = Camera.main;
        interactMessage = GameObject.Find("InteractMessage");
        interactMessage.SetActive(false);
    }

    void Update()
    {
        // Handle general interactions
        if (Input.GetKeyDown(KeyCode.E)) // "E" is the pickup/interaction key
        {
            TryPickupOrInteract();
        }

        // Handle note interaction using raycast
        CheckForNotes();
    }

    void TryPickupOrInteract()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // Ray from the center of the screen

        Debug.Log("only casted");
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactRange, interactionLayerMask))
        {
            Debug.Log("raycasted");
            if (hit.collider.CompareTag("Pickup")) // Ensure the object has the "Pickup" tag
            {
                Debug.Log("dropped and picked up");
                DropObject();
                PickupObject(hit.collider.gameObject);
            }
            else if (heldObject != null)
            {
                Debug.Log("slot");
                if (hit.collider.CompareTag("BatterySlot")) // Ensure the slot has the "BatterySlot" tag
                {
                    Debug.Log("slot beneran");
                    PlaceBatteryInSlot(hit.collider.gameObject);
                }
            }
            else if (hit.collider.CompareTag("OvenButton"))
            {
                Debug.Log("oven open activated");
                StartCoroutine(OpenOven());
            }
            else if (hit.collider.CompareTag("Note")) // Interacting with a note
            {
                if (hit.collider.TryGetComponent(out NoteScript noteScript))
                {
                    if (noteScript != activeNote)
                    {
                        // Close the previously active note if it's still open
                        if (activeNote != null && activeNote.GetNoteStatus())
                        {
                            activeNote.ToggleNote();
                        }

                        activeNote = noteScript;
                        interactMessage.SetActive(true);
                    }

                    // Toggle the note when pressing 'E' if it is the currently active note
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        activeNote.ToggleNote();
                    }
                }
            }
        }
    }


    void CheckForNotes()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // Ray from the center of the screen
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactionLayerMask))
        {
            if (hit.collider.CompareTag("Note")) // Check if the raycast hit a note
            {
                if (!interactMessage.activeSelf)
                {
                    interactMessage.SetActive(true);
                }

                if (hit.collider.TryGetComponent(out NoteScript noteScript))
                {
                    if (noteScript != activeNote)
                    {
                        // Close the previously active note if it's still open
                        if (activeNote != null && activeNote.GetNoteStatus())
                        {
                            activeNote.ToggleNote();
                        }

                        activeNote = noteScript;
                    }
                }
            }
            else
            {
                // Hide the interact message and close the note if the player is not looking at any note
                if (interactMessage.activeSelf)
                {
                    interactMessage.SetActive(false);
                }

                if (activeNote != null && activeNote.GetNoteStatus())
                {
                    activeNote.ToggleNote();
                    activeNote = null;
                }
            }
        }
        else
        {
            // Hide the interact message and close the note if the player is not looking at any note
            if (interactMessage.activeSelf)
            {
                interactMessage.SetActive(false);
            }

            if (activeNote != null && activeNote.GetNoteStatus())
            {
                activeNote.ToggleNote();
                activeNote = null;
            }
        }
    }




    void PickupObject(GameObject obj)
    {
        heldObject = obj;
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Disable physics for the held object
        }
        obj.transform.position = holdPosition.position;
        obj.transform.parent = holdPosition;
    }

    void DropObject()
    {
        if (heldObject != null)
        {
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false; // Re-enable physics
            }
            heldObject.transform.parent = null;
            heldObject = null;
        }
    }

    void PlaceBatteryInSlot(GameObject slot)
    {
        Debug.Log("slot function");
        heldObject.transform.position = slot.transform.position;
        heldObject.transform.rotation = slot.transform.rotation;
        heldObject.transform.parent = null;

        // Disable interaction with the battery
        heldObject.GetComponent<Collider>().enabled = false;
        heldObject.GetComponent<Rigidbody>().isKinematic = true;

        heldObject = null;

        Destroy(slot); // Destroy the slot object after placing the battery
        filledSlots++;

        if (filledSlots >= batterySlotCount)
        {
            ActivateButton();
        }
    }

    void ActivateButton()
    {
        // Change the button's material to indicate it's active
        button.GetComponent<Renderer>().material = activeButtonMaterial;
        // Set button to interactable layer
        button.layer = 8;
    }

    IEnumerator OpenOven()
    {
        ovenDoor.GetComponent<Animator>().enabled = true;
        button.layer = 0;
        yield return new WaitForSeconds(5);
        ovenAnimator.Play("OpenOven");
    }
}
