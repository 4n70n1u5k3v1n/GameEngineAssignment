using SojaExiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxFadeOut : MonoBehaviour
{
    public float fadeDuration = 2.0f; // Duration of the fade
    private bool isTriggered = false;
    private Renderer objectRenderer;
    private Color initialColor;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            initialColor = objectRenderer.material.color;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isTriggered && other.CompareTag("Player")) // Assuming the player has the tag "Player"
        {
            GetComponent<MeshRenderer>().enabled = true;
            isTriggered = true;
            StartCoroutine(FadeOut());
        }
    }

    System.Collections.IEnumerator FadeOut()
    {
        float timer = 0;
        while (timer <= fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1.0f, 0.0f, timer / fadeDuration);
            objectRenderer.material.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }

        // Optionally destroy the object or disable the trigger after fade-out
        //Destroy(gameObject);
        // Or, if you prefer:
        GetComponent<Collider>().enabled = false;
    }
}
