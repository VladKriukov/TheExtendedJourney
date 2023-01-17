using UnityEngine;

public class AnimalAI : MonoBehaviour
{
    public enum MovementType
    {
        Idle, Avoiding, Roaming, Attacking
    }
    public MovementType movementType;

    MovementType prevMovementType;

    [SerializeField] Vector3 destinationPoint;
    [SerializeField] float roamingDistance;
    [SerializeField] float destinationStoppingThreshold;
    [SerializeField] float minIdleTime = 0;
    [SerializeField] float maxIdleTime = 0;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float avoidanceTime;

    Rigidbody rb;
    bool avoidanceDirection;
    float idleTime;
    float currentSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void AvoidObstacle(bool right)
    {
        prevMovementType = movementType;
        movementType = MovementType.Avoiding;
        avoidanceDirection = right;
    }

    public void ResumeMovement()
    {
        movementType = prevMovementType;
    }

    private void Update()
    {
        if (destinationPoint != Vector3.zero)
        {
            rb.velocity = transform.forward * currentSpeed;

            Debug.Log("Angle: " + Vector3.Angle(transform.forward, destinationPoint));
            Debug.Log("Dot: " + Vector3.Dot(transform.position, destinationPoint));

            switch (movementType)
            {
                case MovementType.Avoiding:
                    rb.angularVelocity = new Vector3(0, avoidanceDirection == true ? rotationSpeed : -rotationSpeed, 0);
                    break;
                case MovementType.Roaming:
                    currentSpeed = walkSpeed;
                    rb.angularVelocity = new Vector3(0, Vector3.Dot(transform.right, destinationPoint), 0);
                    break;
                case MovementType.Attacking:
                    currentSpeed = runSpeed;
                    rb.angularVelocity = new Vector3(0, Vector3.Angle(transform.forward, destinationPoint), 0);
                    break;
                default:
                    break;
            }
            if (Vector3.Distance(transform.position, destinationPoint) <= destinationStoppingThreshold)
            {
                idleTime = Random.Range(minIdleTime, maxIdleTime);
                movementType = MovementType.Idle;
                destinationPoint = Vector3.zero;
            }
        }
        else if (movementType == MovementType.Idle)
        {
            idleTime -= Time.deltaTime;
            if (idleTime <= 0)
            {
                CreateRandomDestination();
            }
        }
    }

    void CreateRandomDestination()
    {
        destinationPoint = new Vector3(transform.position.x + Random.Range(0, roamingDistance), transform.position.y, transform.position.z + Random.Range(0, roamingDistance));
        movementType = MovementType.Roaming;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(destinationPoint, 2);
    }
}
