using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigsawCheatButton : MonoBehaviour
{
    //reference to the SelectAndDrop script
    public SelectAndDrop selectAndDrop;

    void Start()
    {
        //ensure the reference to SelectAndDrop is set
        if (selectAndDrop == null)
        {
            selectAndDrop = FindObjectOfType<SelectAndDrop>();
        }
    }

    void OnMouseDown()
    {
        //reset the puzzle pieces to their correct positions
        ResetPieces();

        //check if the puzzle is completed
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
