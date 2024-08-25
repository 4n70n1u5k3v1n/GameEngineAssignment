using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigsawCheatButton : MonoBehaviour
{
    // Reference to the SelectAndDrop script
    public SelectAndDrop selectAndDrop;

    void Start()
    {
        // Ensure the reference to SelectAndDrop is set
        if (selectAndDrop == null)
        {
            selectAndDrop = FindObjectOfType<SelectAndDrop>();
        }
    }

    void OnMouseDown()
    {
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
}
