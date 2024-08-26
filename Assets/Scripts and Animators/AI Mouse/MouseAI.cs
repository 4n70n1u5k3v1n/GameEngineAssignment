using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class MouseAI : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float obstacleRange = 1f;
    [SerializeField] private float rotSpeed = 7f;
    private Animator animator;

    private Quaternion direction;
    private bool isRotating = false;

    private bool isMoving = true;
    private int movingCount = 0;

    [SerializeField] private GameObject player;

    public enum MouseState
    {
        Wander = 0,
        Seek = 1,
    }

    [SerializeField] private MouseState state;

    void Start()
    {
        //start in a random direction
        float angle = Random.Range(-180.0f, 180.0f);
        direction = Quaternion.LookRotation(Quaternion.Euler(0.0f, angle, 0.0f) * transform.forward);
        isRotating = true;
        state = MouseState.Wander;
        animator = GetComponent<Animator>();
        animator.SetTrigger("Walk");
    }

    void OnDrawGizmos()
    {
        //draw a red line gizmo to indicate collision avoidance distance
        Gizmos.color = Color.red;
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 2, transform.position.z);
        Gizmos.DrawLine(origin, origin + transform.forward * obstacleRange);
    }

    void Update()
    {
        DetermineState();

        if (state == MouseState.Seek)
        {
            Seeking();
        }
        else
        {
            Wandering();
        }
    }

    private void DetermineState()
    {
        float xDiff = player.transform.position.x - transform.position.x;
        float zDiff = player.transform.position.z - transform.position.z;
        float planeDiff = Mathf.Sqrt(Mathf.Pow(xDiff, 2) + Mathf.Pow(zDiff, 2));
        float yDiff = Mathf.Abs(player.transform.position.y - transform.position.y);
        if (planeDiff < 7f && yDiff < 0.5f)
        {
            state = MouseState.Seek;
        }
        else
        {
            state = MouseState.Wander;
        }
    }

    void Wandering()
    {
        //if the agent is rotating
        if (isRotating)
        {
            isMoving = false;
            //rotate the agent over several frames
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                   direction, rotSpeed * Time.deltaTime);

            //if the agent within a certain angle of the correct direction
            if (Quaternion.Angle(transform.rotation, direction) < 1.0f)
            {
                isRotating = false;
            }
        }
        else
        {
            isMoving = true;
            movingCount = 0;
            //move the agent
            transform.Translate(0, 0, walkSpeed * Time.deltaTime);
        }

        //collision avoidance
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 2, transform.position.z);
        Ray ray = new Ray(origin, transform.forward);
        RaycastHit hit;

        //cast a sphere to check whether it collides with anything
        if (Physics.SphereCast(ray, 0.2f, out hit))
        {
            //if the collision is within the collision avoidance range
            if (hit.distance < obstacleRange)
            {
                //choose a random angle
                float angle = Random.Range(-110.0f, 110.0f);

                if (!isMoving)
                {
                    movingCount++;

                    if (movingCount > 5)
                    {
                        angle = 45.0f;
                    }
                }

                //set the direction based on the angle
                direction = Quaternion.LookRotation(Quaternion.Euler(0.0f, angle, 0.0f) * transform.forward);
                isRotating = true;
            }
        }
    }

    void Seeking()
    {
        Vector3 toTarget = player.transform.position - transform.position;
        toTarget.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(toTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
        Vector3 desiredVelocity = toTarget.normalized * walkSpeed;
        transform.position += desiredVelocity * Time.deltaTime;
        PlayerCaught();
    }

    void PlayerCaught()
    {
        float xDiff = player.transform.position.x - transform.position.x;
        float zDiff = player.transform.position.z - transform.position.z;
        float planeDiff = Mathf.Sqrt(Mathf.Pow(xDiff, 2) + Mathf.Pow(zDiff, 2));
        if (planeDiff < 0.5f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
