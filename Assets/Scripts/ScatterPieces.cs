using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterPieces : MonoBehaviour
{
    public List<GameObject> puzzlePieces; // List of puzzle pieces to scatter
    public Transform scatterArea; // Transform indicating the scatter area
    public float scatterRadius = 5f; // Radius for scattering pieces

    void Start()
    {
        Scatter();
    }

    void Scatter()
    {
        foreach (GameObject piece in puzzlePieces)
        {
            Vector3 randomPosition = GetRandomPosition();
            piece.transform.position = randomPosition;
        }
    }

    Vector3 GetRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * scatterRadius;
        randomDirection.y = 0; // Keep the pieces on the same plane
        return scatterArea.position + randomDirection;
    }
}
