using SojaExiles;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteraction : MonoBehaviour
{

    FadeInOut fade;

    AudioManager audioManager;

    private void Awake()
    {
        GameObject audioObject = GameObject.FindGameObjectWithTag("Audio");
        if (audioObject != null)
        {
            audioManager = audioObject.GetComponent<AudioManager>();
            if (audioManager == null)
            {
                Debug.LogError("AudioManager component not found on the 'Audio' GameObject.");
            }
        }
        else
        {
            Debug.LogError("GameObject named 'Audio' not found.");
        }
    }
    //variables needed to shoot rays
    private Camera playerCamera;
    public float interactRange = 0.7f; //max range to interact with objects
    public LayerMask interactionLayerMask; //layer mask to define what objects are interactable

    //variables needed to pick up objects
    public Transform holdPosition;
    private GameObject heldObject;

    //variables needed for battery machine and button
    public GameObject button; 
    public Material activeButtonMaterial;
    private int batterySlotCount = 4;
    private int filledSlots = 0;
    [SerializeField] GameObject ovenDoor;
    [SerializeField] private Animator ovenAnimator;

    //variables for note interaction
    private NoteScript activeNote;
    private GameObject interactMessage;

    //variables for potion
    private bool ovenOpened;

    void Start()
    {
        fade = FindObjectOfType<FadeInOut>();
        playerCamera = Camera.main;
        interactMessage = GameObject.Find("InteractMessage");
        interactMessage.SetActive(false);
        ovenOpened = false;
    }

    void Update()
    {
        //handle interactables using raycast
        CheckForInteractables();

        if (Input.GetKeyDown(KeyCode.E)) //"E" for pickup/interaction key
        {
            TryPickupOrInteract();
        }
    }

    void TryPickupOrInteract()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); // Ray from the center of the screen

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactRange, interactionLayerMask))
        {
            if (hit.collider.CompareTag("Pickup")) // Ensure the object has the "Pickup" tag
            {
                DropObject();
                PickupObject(hit.collider.gameObject);
            }
            else if (heldObject != null)
            {
                if (hit.collider.CompareTag("BatterySlot")) // Ensure the slot has the "BatterySlot" tag
                {
                    PlaceBatteryInSlot(hit.collider.gameObject);
                }
            }
            else if (hit.collider.CompareTag("OvenButton"))
            {
                StartCoroutine(OpenOven());
                audioManager.PlaySFX(audioManager.ButtonPress);
            }
            else if (hit.collider.CompareTag("Note")) //interacting with a note
            {
                if (hit.collider.TryGetComponent(out NoteScript noteScript))
                {
                    if (noteScript != activeNote)
                    {
                        //close the previously active note if it's still open
                        if (activeNote != null && activeNote.GetNoteStatus())
                        {
                            activeNote.ToggleNote();
                        }

                        activeNote = noteScript;
                        interactMessage.SetActive(true);
                    }

                    //toggle the note when pressing 'E' if it is the currently active note
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        activeNote.ToggleNote();
                    }
                }
            }
            else if (hit.collider.CompareTag("Potion") && ovenOpened) //allow interaction with potion if oven opened
            {
                StartCoroutine(ChangeScene());
            }
        }
    }


    void CheckForInteractables()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)); //ray from the center of the screen
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactionLayerMask))
        {
            //check if the raycast hit any interactable objects
            if (hit.collider.CompareTag("Note") || hit.collider.CompareTag("BatterySlot") || hit.collider.CompareTag("Pickup") || hit.collider.CompareTag("BatterySlot") || hit.collider.CompareTag("Potion") || hit.collider.CompareTag("OvenButton"))
            {
                if (!interactMessage.activeSelf)
                {
                    interactMessage.SetActive(true);
                }

                if (hit.collider.TryGetComponent(out NoteScript noteScript))
                {
                    if (noteScript != activeNote)
                    {
                        //lose the previously active note if it's still open
                        if (activeNote != null && activeNote.GetNoteStatus())
                        {
                            activeNote.ToggleNote();
                        }

                        activeNote = noteScript;
                    }
                }
            }
        }
        else
        {
            //hide the interact message and close the note if the player is not looking at any note
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
        audioManager.PlaySFX(audioManager.BatteryPickup);
        heldObject = obj;
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; //disable physics for the held object
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
                rb.isKinematic = false; //re-enable physics
            }
            heldObject.transform.parent = null;
            heldObject = null;
        }
    }

    void PlaceBatteryInSlot(GameObject slot)
    {
        audioManager.PlaySFX(audioManager.BatteryInsertion);
        heldObject.transform.position = slot.transform.position;
        heldObject.transform.rotation = slot.transform.rotation;
        heldObject.transform.parent = null;

        //disable interaction with the battery
        heldObject.GetComponent<Collider>().enabled = false;
        heldObject.GetComponent<Rigidbody>().isKinematic = true;

        heldObject = null;

        Destroy(slot); //destroy the slot object after placing the battery
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
        audioManager.PlaySFX(audioManager.OvenDing);
        ovenDoor.GetComponent<Animator>().enabled = true;
        button.layer = 0;
        yield return new WaitForSeconds(5);
        ovenAnimator.Play("OpenOven");
        ovenOpened = true;
    }

    public IEnumerator ChangeScene()
    {
        audioManager.PlaySFX(audioManager.Magic);
        fade.FadeIn();
        yield return new WaitForSeconds(16);
        SceneManager.LoadScene("TBC");
    }
}
