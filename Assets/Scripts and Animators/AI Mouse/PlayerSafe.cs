using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSafe : MonoBehaviour
{
    [SerializeField] private GameObject mouse;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            mouse.GetComponent<MouseAI>().playerOnFirstFloor = false;
        }
    }
}
