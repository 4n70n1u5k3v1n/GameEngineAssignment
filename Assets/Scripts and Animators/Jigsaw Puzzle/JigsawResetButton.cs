using UnityEngine;

public class JigsawResetButton : MonoBehaviour
{
    //reference to the puzzle pieces
    public GameObject[] puzzlePieces;
    public Transform scatterArea; //transform indicating the scatter area
    public float scatterRadius = 5f; //radius for scattering pieces

    void OnMouseDown()
    {
        //scatter the puzzle pieces
        Scatter();
    }

    void Scatter()
    {
        foreach (GameObject piece in puzzlePieces)
        {
            Vector3 randomPosition = GetRandomPosition();
            randomPosition.y = piece.transform.position.y; //preserve the original y value
            piece.transform.position = randomPosition;
        }
    }

    Vector3 GetRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * scatterRadius;
        randomDirection.y = 0; //keep the pieces on the same plane
        return scatterArea.position + randomDirection;
    }
}