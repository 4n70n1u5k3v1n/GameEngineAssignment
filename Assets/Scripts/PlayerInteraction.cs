using SojaExiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction: MonoBehaviour
{
    //variable needed to shoot rays
    private Camera playerCamera;
    public float interactRange = 5f; //the maximum range to interact with objects
    public LayerMask interactionLayerMask; //layer mask to define what objects are interactable

    //variable needed to pick up objects
    public Transform holdPosition; //the position where the object will be held
    private GameObject heldObject; //the object currently being held

    //variable needed for battery machine and button
    public GameObject button; //the button that should turn green and become interactable
    public Material activeButtonMaterial; //material to apply when the button becomes active
    private int batterySlotCount = 4; // Number of battery slots
    private int filledSlots = 0; // Number of filled battery slots
    [SerializeField] GameObject ovenDoor;

    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) //"E" is the pickup/interaction key
        {
            TryPickupOrInteract();
        }
    }

    void TryPickupOrInteract()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // Ray from the center of the screen

        Debug.Log("only casted");
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactRange, interactionLayerMask))
        {
            Debug.Log("raycasted");
            // Try to pick up an object
            if (hit.collider.CompareTag("Pickup")) // Ensure the object has the "Pickup" tag
            {
                Debug.Log("dropped and picked up");
                DropObject();
                PickupObject(hit.collider.gameObject);
            }
            else if (heldObject != null)
            {
                Debug.Log("slot");
                // Try to place the battery in a battery slot
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
        //change the button's material to indicate it's active
        button.GetComponent<Renderer>().material = activeButtonMaterial;
        //set button to interactable layer
        button.layer = 8;
    }

    IEnumerator OpenOven()
    {
        Debug.Log("before 5");
        new WaitForSeconds(5);
        Debug.Log("after 5");
        ovenDoor.GetComponent<Animator>().enabled = true;
        ovenDoor.GetComponent<OvenFlip>().enabled = true;
        button.layer = 0;
        Debug.Log("after all");
        yield return null;
    }
}