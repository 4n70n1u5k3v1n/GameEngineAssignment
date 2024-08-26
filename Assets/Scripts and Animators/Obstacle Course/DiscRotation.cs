using UnityEngine;

public class DiscRotation: MonoBehaviour
{
    public float spinSpeed = 100f; //adjust the speed here
    public bool clockwise = true;  //set to true for clockwise rotation

    void Update()
    {
        float direction = clockwise ? -1f : 1f;
        transform.Rotate(0f, spinSpeed * direction * Time.deltaTime, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) //assumes your character has the tag "Player"
        {
            other.transform.SetParent(transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}

