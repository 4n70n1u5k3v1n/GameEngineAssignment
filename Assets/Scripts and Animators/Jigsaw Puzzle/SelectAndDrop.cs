using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAndDrop : MonoBehaviour
{
    AudioManager audioManager;

    private void Awake()
    {
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
            Debug.LogError("GameObject named 'Audio' not found.");
        }
    }

    private GameObject selectedPiece;
    private bool isDragging = false;
    public Camera mainCamera; // Reference to the jigsaw camera
    public float snapThreshold = 0.8f; // Adjust this value as needed
    public Dictionary<GameObject, Vector3> correctPositions = new Dictionary<GameObject, Vector3>();

    // Reference to the door GameObject and Battery inside it
    public GameObject door;
    private Animator doorAnimator;
    private MonoBehaviour doorScript;
    public GameObject battery;

    // Reference to the snap audio clip
    public AudioClip snapAudioClip;

    // Reference to puzzle completion audio clip
    public AudioClip puzzleCompletionAudioClip;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("JigsawCamera").GetComponent<Camera>();

        // Initialize correct positions for each puzzle piece using local positions
        correctPositions.Add(GameObject.Find("Piece (0)"), new Vector3(-4.3432f, 1.2459f, 4.9769f));
        correctPositions.Add(GameObject.Find("Piece (1)"), new Vector3(-4.3549f, 1.2459f, 5.0566f));
        correctPositions.Add(GameObject.Find("Piece (2)"), new Vector3(-4.3432f, 1.2459f, 5.1371f));
        correctPositions.Add(GameObject.Find("Piece (3)"), new Vector3(-4.3549f, 1.2459f, 5.2176f));
        correctPositions.Add(GameObject.Find("Piece (4)"), new Vector3(-4.3428f, 1.2459f, 5.298f));
        correctPositions.Add(GameObject.Find("Piece (5)"), new Vector3(-4.3544f, 1.2459f, 5.3667f));
        correctPositions.Add(GameObject.Find("Piece (6)"), new Vector3(-4.2742f, 1.2459f, 5.3787f));
        correctPositions.Add(GameObject.Find("Piece (7)"), new Vector3(-4.1936f, 1.2459f, 5.3664f));
        correctPositions.Add(GameObject.Find("Piece (8)"), new Vector3(-4.1125f, 1.2459f, 5.3784f));
        correctPositions.Add(GameObject.Find("Piece (9)"), new Vector3(-4.0317f, 1.2459f, 5.3663f));
        correctPositions.Add(GameObject.Find("Piece (10)"), new Vector3(-3.9628f, 1.2459f, 5.3779f));
        correctPositions.Add(GameObject.Find("Piece (11)"), new Vector3(-4.2742f, 1.2459f, 5.2983f));
        correctPositions.Add(GameObject.Find("Piece (12)"), new Vector3(-4.2774f, 1.2459f, 5.2175f));
        correctPositions.Add(GameObject.Find("Piece (13)"), new Vector3(-4.2752f, 1.2459f, 5.1368f));
        correctPositions.Add(GameObject.Find("Piece (14)"), new Vector3(-4.2749f, 1.2459f, 4.9887f));
        correctPositions.Add(GameObject.Find("Piece (15)"), new Vector3(-4.194f, 1.2459f, 4.9765f));
        correctPositions.Add(GameObject.Find("Piece (16)"), new Vector3(-4.1129f, 1.2459f, 4.9887f));
        correctPositions.Add(GameObject.Find("Piece (17)"), new Vector3(-4.0318f, 1.2459f, 4.9765f));
        correctPositions.Add(GameObject.Find("Piece (18)"), new Vector3(-3.9519f, 1.2459f, 4.9885f));
        correctPositions.Add(GameObject.Find("Piece (19)"), new Vector3(-4.2747f, 1.2459f, 5.057f));
        correctPositions.Add(GameObject.Find("Piece (20)"), new Vector3(-3.9509f, 1.2459f, 5.2983f));
        correctPositions.Add(GameObject.Find("Piece (21)"), new Vector3(-3.963f, 1.2459f, 5.2179f));
        correctPositions.Add(GameObject.Find("Piece (22)"), new Vector3(-3.9509f, 1.2459f, 5.1376f));
        correctPositions.Add(GameObject.Find("Piece (23)"), new Vector3(-3.9634f, 1.2459f, 5.0572f));
        correctPositions.Add(GameObject.Find("Piece (24)"), new Vector3(-4.1939f, 1.2459f, 5.2981f));
        correctPositions.Add(GameObject.Find("Piece (25)"), new Vector3(-4.1126f, 1.2459f, 5.2983f));
        correctPositions.Add(GameObject.Find("Piece (26)"), new Vector3(-4.0315f, 1.2459f, 5.2981f));
        correctPositions.Add(GameObject.Find("Piece (27)"), new Vector3(-4.1939f, 1.2459f, 5.2173f));
        correctPositions.Add(GameObject.Find("Piece (28)"), new Vector3(-4.1129f, 1.2459f, 5.2175f));
        correctPositions.Add(GameObject.Find("Piece (29)"), new Vector3(-4.0318f, 1.2459f, 5.2173f));
        correctPositions.Add(GameObject.Find("Piece (30)"), new Vector3(-4.1946f, 1.2459f, 5.1375f));
        correctPositions.Add(GameObject.Find("Piece (31)"), new Vector3(-4.1136f, 1.2459f, 5.1368f));
        correctPositions.Add(GameObject.Find("Piece (32)"), new Vector3(-4.0323f, 1.2459f, 5.1375f));
        correctPositions.Add(GameObject.Find("Piece (33)"), new Vector3(-4.1944f, 1.2459f, 5.0566f));
        correctPositions.Add(GameObject.Find("Piece (34)"), new Vector3(-4.1133f, 1.2459f, 5.0561f));
        correctPositions.Add(GameObject.Find("Piece (35)"), new Vector3(-4.0322f, 1.2459f, 5.0563f));

        // Initialize door references
        if (door != null)
        {
            // Ensure both components are initially disabled
            if (doorAnimator != null)
            {
                doorAnimator.enabled = false;
            }
            if (doorScript != null)
            {
                doorScript.enabled = false;
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.transform.CompareTag("Puzzle"))
                {
                    selectedPiece = hit.transform.gameObject;
                    isDragging = true;
                    Debug.Log($"Selected piece: {selectedPiece.name}");
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (selectedPiece != null)
            {
                isDragging = false;

                // Project the mouse point in world space and convert to local position
                Vector3 mousePoint = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.WorldToScreenPoint(selectedPiece.transform.position).z));
                selectedPiece.transform.localPosition = selectedPiece.transform.parent.InverseTransformPoint(mousePoint);

                // Check if the piece is close enough to its correct position
                if (correctPositions.ContainsKey(selectedPiece))
                {
                    Vector3 correctPosition = correctPositions[selectedPiece];
                    if (Vector3.Distance(selectedPiece.transform.localPosition, correctPosition) <= snapThreshold)
                    {
                        // Snap to the correct position
                        selectedPiece.transform.localPosition = correctPosition;
                        PlaySnapAudio(); // Play snap audio
                    }
                }

                selectedPiece = null;

                // Check if the puzzle is completed
                CheckPuzzleCompletion();
            }
        }

        if (isDragging && selectedPiece != null)
        {
            Vector3 mousePoint = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.WorldToScreenPoint(selectedPiece.transform.position).z));
            selectedPiece.transform.localPosition = selectedPiece.transform.parent.InverseTransformPoint(mousePoint);
        }
    }

    // Method to play the snap audio
    private void PlaySnapAudio()
    {
        if (audioManager != null && snapAudioClip != null)
        {
            audioManager.PlaySFX(snapAudioClip);
        }
    }

    public void CheckPuzzleCompletion()
    {
        bool isPuzzleComplete = true;

        foreach (var piece in correctPositions)
        {
            if (Vector3.Distance(piece.Key.transform.localPosition, piece.Value) > snapThreshold)
            {
                isPuzzleComplete = false;
                break;
            }
        }

        doorAnimator = door.GetComponent<Animator>();

        // Enable or disable the door's animator based on the puzzle's state
        if (doorAnimator != null)
        {
            doorAnimator.enabled = isPuzzleComplete;
        }

        if (doorAnimator.isActiveAndEnabled)
        {
            audioManager.PlaySFX(audioManager.GlassDoor);
            doorAnimator.Play("Opening 1");
            battery.layer = 8;

            // Start coroutine to play completion audio after 1.5 seconds
            StartCoroutine(PlayCompletionAudioWithDelay(1.5f));
        }
    }

    // Coroutine to play the completion audio after a delay
    private IEnumerator PlayCompletionAudioWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (audioManager != null && puzzleCompletionAudioClip != null)
        {
            audioManager.PlaySFX(puzzleCompletionAudioClip);
        }
    }
}
