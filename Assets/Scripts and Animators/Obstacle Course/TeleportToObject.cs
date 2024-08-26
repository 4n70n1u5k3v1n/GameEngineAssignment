using UnityEngine;

public class TeleportToObject : MonoBehaviour
{
    public Transform targetObject;
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (targetObject != null)
            {
                Debug.Log("Teleporting to: " + targetObject.position);
                characterController.enabled = false;
                transform.position = targetObject.position;
                characterController.enabled = true;
            }
            else
            {
                Debug.LogWarning("Target object is not assigned.");
            }
        }
    }
}




