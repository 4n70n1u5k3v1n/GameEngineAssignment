using SojaExiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkPlayer : MonoBehaviour
{
    [SerializeField] Vector3 shrinkSize = new Vector3(0.2f, 0.2f, 0.2f);
    [SerializeField] float shrinkOffset = 0.2f;
    [SerializeField] float shrinkSpeed = 3f;
    [SerializeField] float shrinkJumpSpeed = 3f;
    [SerializeField] float shrinkGravity = -2f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Entered if");
            other.transform.localScale = Vector3.Scale(other.transform.localScale, shrinkSize);
            other.GetComponent<CharacterController>().stepOffset = shrinkOffset;
            other.GetComponent<CharacterMovement1>().speed = shrinkSpeed;
            other.GetComponent<CharacterMovement1>().jumpSpeed = shrinkJumpSpeed;
            other.GetComponent<CharacterMovement1>().gravity = shrinkGravity;
            GetComponent<ShrinkPlayer>().enabled = false;
        }
    }
}
