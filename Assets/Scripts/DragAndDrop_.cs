using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DragAndDrop_ : MonoBehaviour
{
    public GameObject SelectedPiece;
    private bool isDragging = false;
    public Camera mainCamera;
    public float snapThreshold = 0.8f; // Adjust this value as needed
    private Dictionary<GameObject, Vector3> correctPositions = new Dictionary<GameObject, Vector3>();
    public float zoomSpeed = 10f; // Speed of zooming

    void Start()
    {
        mainCamera = Camera.main;

        // Initialize correct positions for each puzzle piece using local positions
        correctPositions.Add(GameObject.Find("Piece (0)"), new Vector3(-4.3432f, 1.2459f, 4.9769f));
        correctPositions.Add(GameObject.Find("Piece (1)"), new Vector3(-4.3549f, 1.2459f, 5.0566f));
        correctPositions.Add(GameObject.Find("Piece (2)"), new Vector3(-4.3432f, 1.2459f, -0.684f));
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
                    SelectedPiece = hit.transform.gameObject;
                    isDragging = true;
                    Debug.Log($"Selected piece: {SelectedPiece.name}");
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (SelectedPiece != null)
            {
                isDragging = false;
                // Ensure the piece lays flat on the surface
                Vector3 MousePoint = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.WorldToScreenPoint(SelectedPiece.transform.position).z));
                SelectedPiece.transform.localPosition = SelectedPiece.transform.parent.InverseTransformPoint(MousePoint);

                // Check if the piece is close enough to its correct position
                if (correctPositions.ContainsKey(SelectedPiece))
                {
                    Vector3 correctPosition = correctPositions[SelectedPiece];
                    if (Vector3.Distance(SelectedPiece.transform.localPosition, correctPosition) <= snapThreshold)
                    {
                        // Snap to the correct position
                        SelectedPiece.transform.localPosition = correctPosition;
                    }
                }

                SelectedPiece = null;
            }
        }

        if (isDragging && SelectedPiece != null)
        {
            // Update the position of the selected piece to follow the mouse cursor
            Vector3 MousePoint = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.WorldToScreenPoint(SelectedPiece.transform.position).z));
            SelectedPiece.transform.localPosition = SelectedPiece.transform.parent.InverseTransformPoint(MousePoint);
        }

        // Handle zooming with the mouse scroll wheel
        if (SelectedPiece != null)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0.0f)
            {
                Vector3 newPosition = SelectedPiece.transform.localPosition + mainCamera.transform.forward * scroll * zoomSpeed;
                SelectedPiece.transform.localPosition = newPosition;
            }
        }
    }
}
