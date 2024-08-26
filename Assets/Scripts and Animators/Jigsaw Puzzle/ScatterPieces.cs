using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterPieces : MonoBehaviour
{
    public List<GameObject> puzzlePieces; //list of puzzle pieces to scatter
    public Transform scatterArea; //transform indicating the scatter area
    public float scatterRadius = 5f; //radius for scattering pieces

    void Start()
    {
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