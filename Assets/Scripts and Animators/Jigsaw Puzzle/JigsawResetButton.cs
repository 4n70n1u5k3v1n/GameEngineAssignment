using UnityEngine;

public class JigsawResetButton : MonoBehaviour
{
    // Reference to the puzzle pieces
    public GameObject[] puzzlePieces;
    public Transform scatterArea; // Transform indicating the scatter area
    public float scatterRadius = 5f; // Radius for scattering pieces

    // Reference to the reset audio clip
    public AudioClip resetAudioClip;

    // Reference to the AudioSource component
    private AudioSource audioSource;

    void Start()
    {
        // Get or add the AudioSource component
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
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
        if (resetAudioClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(resetAudioClip);
        }
    }
}
