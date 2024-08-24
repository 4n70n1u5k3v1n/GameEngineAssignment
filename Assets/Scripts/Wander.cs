using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Steering;
using static UnityEngine.GraphicsBuffer;

public class Wander : MonoBehaviour
{
    public float speed = 2f;
    public float obstacleRange = 1f;
    public float rotSpeed = 7f;
    private Animator animator;

    Quaternion direction;       // wandering direction
    bool isRotating = false;    // rotate over a number of frames

    bool isMoving = true;
    int movingCount = 0;

    public GameObject player;
    bool hasTarget = false;
    public float maxSpeed = 5f;
    float mass = 1f;
    Vector3 currentVelocity = Vector3.zero;

    public enum MouseState
    {
        Wander = 0,
        Seek = 1,
    }

    public MouseState state;

    // Start is called before the first frame update
    void Start()
    {
        // start in a random direction
        float angle = Random.Range(-180.0f, 180.0f);
        direction = Quaternion.LookRotation(Quaternion.Euler(0.0f, angle, 0.0f) * transform.forward);
        isRotating = true;
        state = MouseState.Wander;
        animator = GetComponent<Animator>();
        animator.SetTrigger("Walk");
    }

    void OnDrawGizmos()
    {
        // draw a red line gizmo to indicate collision avoidance distance
        Gizmos.color = Color.red;
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 2, transform.position.z);
        Gizmos.DrawLine(origin, origin + transform.forward * obstacleRange);
    }

    // Update is called once per frame
    void Update()
    {
        /*if ((player.transform.position.x - transform.position.x < 7f || player.transform.position.z - transform.position.z < 7f) && player.transform.position.y)

        if (hasTarget)
        {
            Vector3 steeringForce;
            if (state == MouseState.Seek)
            {
                steeringForce = Seeking();
            }
            else
            {
                Wandering();
            }

            Vector3 acceleration = steeringForce / mass;
            currentVelocity += acceleration * Time.deltaTime;
            currentVelocity = Vector3.ClampMagnitude(currentVelocity, maxSpeed);
            transform.position += currentVelocity * Time.deltaTime;

            if (currentVelocity != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(currentVelocity);
            }
        }*/
        Wandering();
    }

    void Wandering()
    {
        // if the agent is rotating
        if (isRotating)
        {
            isMoving = false;
            // rotate the agent over several frames
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                   direction, rotSpeed * Time.deltaTime);

            // if the agent within a certain angle of the correct direction
            if (Quaternion.Angle(transform.rotation, direction) < 1.0f)
            {
                isRotating = false;
            }
        }
        else
        {
            isMoving = true;
            movingCount = 0;
            // move the agent
            transform.Translate(0, 0, speed * Time.deltaTime);
        }

        // collision avoidance
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 2, transform.position.z);
        Ray ray = new Ray(origin, transform.forward);
        RaycastHit hit;

        // cast a sphere to check whether it collides with anything
        if (Physics.SphereCast(ray, 0.2f, out hit))
        {
            // if the collision is within the collision avoidance range
            if (hit.distance < obstacleRange)
            {
                // choose a random angle
                float angle = Random.Range(-110.0f, 110.0f);

                if (!isMoving)
                {
                    movingCount++;

                    if (movingCount > 5)
                    {
                        angle = 45.0f;
                    }
                }

                // set the direction based on the angle
                direction = Quaternion.LookRotation(Quaternion.Euler(0.0f, angle, 0.0f) * transform.forward);
                isRotating = true;
            }
        }
    }

    /*Vector3 Seeking()
    {
        Vector3 toTarget = target.position - transform.position;
        toTarget.y = 0;
        Vector3 desiredVelocity = toTarget.normalized * maxSpeed;
        state = SteeringState.Arrive;
        return (desiredVelocity - currentVelocity);
    }*/
}
