using SojaExiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkPlayer : MonoBehaviour
{
    [SerializeField] Vector3 shrinkSize = new Vector3(0.2f, 0.2f, 0.2f);
    [SerializeField] float shrinkOffset = 0.2f;
    [SerializeField] float shrinkSpeed = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.localScale = Vector3.Scale(other.transform.localScale, shrinkSize);
            other.GetComponent<CharacterController>().stepOffset = shrinkOffset;
            other.GetComponent<PlayerMovement>().speed = shrinkSpeed;
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<ShrinkPlayer>().enabled = false;
        }
    }
}
