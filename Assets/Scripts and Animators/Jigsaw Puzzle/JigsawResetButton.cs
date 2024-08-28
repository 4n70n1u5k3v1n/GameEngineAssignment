using UnityEngine;

public class JigsawResetButton : MonoBehaviour
{
    // Reference to the puzzle pieces
    public GameObject[] puzzlePieces;
    public Transform scatterArea; // Transform indicating the scatter area
    public float scatterRadius = 5f; // Radius for scattering pieces

    // Reference to the AudioManager
    private AudioManager audioManager;

    void Start()
    {
        // Find the AudioManager in the scene
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
            Debug.LogError("GameObject with tag 'Audio' not found.");
        }
    }

    void OnMouseDown()
    {
        // Scatter the puzzle pieces
        Scatter();

        // Play the reset audio
        PlayResetAudio();
    }

    void Scatter()
    {
        foreach (GameObject piece in puzzlePieces)
        {
            Vector3 randomPosition = GetRandomPosition();
            randomPosition.y = piece.transform.position.y; // Preserve the original y value
            piece.transform.position = randomPosition;
        }
    }

    Vector3 GetRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * scatterRadius;
        randomDirection.y = 0; // Keep the pieces on the same plane
        return scatterArea.position + randomDirection;
    }

    // Method to play the reset audio
    private void PlayResetAudio()
    {
        if (audioManager != null && audioManager.ResetAudioClip != null)
        {
            audioManager.PlaySFX(audioManager.ResetAudioClip);
        }
    }
}
