using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public float pickupRange = 3f; // The maximum range to pick up objects
    public Transform holdPosition; // The position where the object will be held
    public LayerMask pickupLayerMask; // Layer mask to define what can be picked up
    public GameObject button; // The button that should turn green and become interactable
    public Material activeButtonMaterial; // Material to apply when the button becomes active

    private GameObject heldObject; // The object currently being held
    private Camera playerCamera;
    private int batterySlotCount = 4; // Number of battery slots
    private int filledSlots = 0; // Number of filled battery slots

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

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, pickupRange, pickupLayerMask))
        {
            if (heldObject == null)
            {
                // Try to pick up an object
                if (hit.collider.CompareTag("Pickup")) // Ensure the object has the "Pickup" tag
                {
                    PickupObject(hit.collider.gameObject);
                }
            }
            else
            {
                Debug.Log("else");
                // Try to place the battery in a battery slot
                if (hit.collider.CompareTag("BatterySlot")) // Ensure the slot has the "BatterySlot" tag
                {
                    Debug.Log("batteryslot");
                    PlaceBatteryInSlot(hit.collider.gameObject);
                }
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
        // Additional logic to make the button pressable can be added here
    }
}