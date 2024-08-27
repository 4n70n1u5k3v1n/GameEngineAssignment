using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigsawCheatButton : MonoBehaviour
{
    // Reference to the SelectAndDrop script
    public SelectAndDrop selectAndDrop;

    // Reference to the cheat audio clip
    public AudioClip cheatAudioClip;

    // Reference to the AudioSource component
    private AudioSource audioSource;

    void Start()
    {
        // Ensure the reference to SelectAndDrop is set
        if (selectAndDrop == null)
        {
            selectAndDrop = FindObjectOfType<SelectAndDrop>();
        }

        // Get or add the AudioSource component
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnMouseDown()
    {
        // Play the cheat audio
        PlayCheatAudio();

        // Reset the puzzle pieces to their correct positions
        ResetPieces();

        // Check if the puzzle is completed
        selectAndDrop.CheckPuzzleCompletion();
    }

    void ResetPieces()
    {
        foreach (var piece in selectAndDrop.correctPositions)
        {
            piece.Key.transform.localPosition = piece.Value;
        }
    }

    // Method to play the cheat audio
    private void PlayCheatAudio()
    {
        if (cheatAudioClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(cheatAudioClip);
        }
    }
}
