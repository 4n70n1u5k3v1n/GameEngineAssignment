using SojaExiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxFadeOut : MonoBehaviour
{
    private bool isTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (!isTriggered && other.CompareTag("Player"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
            isTriggered = true;
            GetComponent<Collider>().enabled = false;
        }
    }
}
