using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigsawCheatButton : MonoBehaviour
{
    // Reference to the SelectAndDrop script
    public SelectAndDrop selectAndDrop;

    // Reference to the AudioManager
    private AudioManager audioManager;

    void Start()
    {
        // Ensure the reference to SelectAndDrop is set
        if (selectAndDrop == null)
        {
            selectAndDrop = FindObjectOfType<SelectAndDrop>();
        }

        // Find the AudioManager
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager component not found in the scene.");
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
        if (audioManager != null && audioManager.CheatAudioClip != null)
        {
            audioManager.PlaySFX(audioManager.CheatAudioClip);
        }
    }
}
