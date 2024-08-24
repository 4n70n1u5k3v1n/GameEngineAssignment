using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDie : MonoBehaviour
{
    public Image fadeImage; // Assign the Image component from the Canvas in the Inspector
    public float fadeDuration = 2f; // Duration of the fade-out effect

    void Start()
    {
        // Ensure the image is fully transparent at the start
        Color color = fadeImage.color;
        color.a = 0;
        fadeImage.color = color;
    }

    public void StartFadeOut()
    {
        StartCoroutine(FadeToBlack());
    }

    private IEnumerator FadeToBlack()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration); // Gradually increase alpha
            fadeImage.color = color;
            yield return null; // Wait for the next frame
        }

        // Ensure the alpha is fully opaque at the end
        color.a = 1;
        fadeImage.color = color;
    }
}
