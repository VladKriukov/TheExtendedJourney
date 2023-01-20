using UnityEngine;

public class AnimalAI : MonoBehaviour
{
    public enum MovementType
    {
        Idle, Avoiding, Roaming, Attacking
    }
    public MovementType movementType;
    MovementType prevMovementType;

    public Transform chaseTarget;
    [SerializeField] float losingSightDistance;
    [SerializeField] Vector3 destinationPoint;
    [SerializeField] float roamingDistance;
    [SerializeField] float destinationStoppingThreshold;
    [SerializeField] float minIdleTime = 0;
    [SerializeField] float maxIdleTime = 0;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float avoidanceTime;
    [SerializeField] LayerMask layerMask;

    Rigidbody rb;
    bool avoidanceDirection;
    float idleTime;
    float currentSpeed;

    RaycastHit hit;
    bool hitting;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chaseTarget = other.gameObject.transform;
            destinationPoint = chaseTarget.position;
        }
    }

    public void AvoidObstacle(bool right)
    {
        prevMovementType = movementType;
        movementType = MovementType.Avoiding;
        avoidanceDirection = right;
    }

    public void ResumeMovement()
    {
        movementType = MovementType.Roaming;
    }

    private void Update()
    {
        if (destinationPoint != Vector3.zero)
        {
            if (chaseTarget != null && Vector3.Distance(transform.position, chaseTarget.position) < losingSightDistance)
            {
                movementType = MovementType.Attacking;
                destinationPoint = chaseTarget.position;
            }
            else
            {
                movementType = MovementType.Roaming;
                chaseTarget = null;
            }
            rb.velocity = transform.forward * currentSpeed + Vector3.down;
            //rb.velocity += Vector3.down * 2f;

            Vector3 targetDirection = destinationPoint - transform.position;
            float singleStep = rotationSpeed * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            Debug.DrawRay(transform.position, newDirection, Color.red);

            switch (movementType)
            {
                case MovementType.Avoiding:
                    rb.angularVelocity = new Vector3(0, avoidanceDirection == true ? rotationSpeed : -rotationSpeed, 0);
                    break;
                case MovementType.Roaming:
                    currentSpeed = walkSpeed;
                    transform.rotation = Quaternion.LookRotation(newDirection);
                    //transform.rotation = new Quaternion(0, transform.rotation.y, 0, 1);
                    break;
                case MovementType.Attacking:
                    currentSpeed = runSpeed;
                    transform.rotation = Quaternion.LookRotation(newDirection);
                    //transform.rotation = new Quaternion(0, transform.rotation.y, 0, 1);
                    break;
                default:
                    break;
            }
            if (Vector3.Distance(transform.position, destinationPoint) <= destinationStoppingThreshold)
            {
                if (movementType == MovementType.Roaming)
                {
                    idleTime = Random.Range(minIdleTime, maxIdleTime);
                    movementType = MovementType.Idle;
                    destinationPoint = Vector3.zero;
                }
                else
                {
                    // attack
                }
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
        else
        {
            CreateRandomDestination();
        }
    }

    // todo: fix so it doesn't go in the water (convert to IEnumerator)
    void CreateRandomDestination()
    {
        destinationPoint = new Vector3(transform.position.x + Random.Range(0, roamingDistance), transform.position.y, transform.position.z + Random.Range(0, roamingDistance));
        hitting = Physics.Raycast(destinationPoint + new Vector3(0, 50, 0), Vector3.down, out hit, 200, layerMask);
        if (hitting == true)
        {
            destinationPoint = new Vector3(destinationPoint.x, hit.point.y, destinationPoint.z);
        }
        movementType = MovementType.Roaming;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(destinationPoint, 2);
    }
}
