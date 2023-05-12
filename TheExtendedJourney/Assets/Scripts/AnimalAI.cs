using UnityEngine;

public class AnimalAI : MonoBehaviour
{
    public enum MovementType
    {
        Idle, Avoiding, Roaming, Attacking, Fleeing
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
    [SerializeField] float attackDelay;
    [SerializeField] float attackDamage;
    [SerializeField] float dragMultiplier = 3;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float attackBuff;
    [SerializeField] float fleeDistance;//This is how close the enemy needs to be to the train before it runs away

    Rigidbody rb;
    bool avoidanceDirection;
    float idleTime;
    float attackTime;
    float currentSpeed;

    RaycastHit hit;
    bool hitting;
    bool runFromTrain;
    Game game;
    Train trainScript;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        game = FindObjectOfType<Game>();
        trainScript = FindObjectOfType<Train>();
    }
    

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                chaseTarget = other.gameObject.transform;
                destinationPoint = chaseTarget.position;
                break;
            case "Train":
                
                break;
            default:
                break;
        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Train"))
        {
            Debug.Log("TRain");
            if (collision.collider.transform.parent.GetComponent<Rigidbody>().velocity.z > 5)
            {
                GetComponent<Resource>().DropLoot();
            }
        }
    }This has been disabled so that the wolves only take damage and can be killed if the train is moving */

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
        runFromTrain = trainScript.trainOn; //This is so the ai knows if the train is active and if it is can run away from it
        if (destinationPoint != Vector3.zero)
        {
            if (chaseTarget != null && Vector3.Distance(transform.position, chaseTarget.position) < losingSightDistance)
            {
                movementType = MovementType.Attacking;
                destinationPoint = chaseTarget.position;
            }
            if (runFromTrain && Vector3.Distance(transform.position, trainScript.gameObject.transform.position) <= fleeDistance)//This checks to see if the train is on and if the enemy is in range of it, if it is th eenemy runs away
            {
                movementType = MovementType.Fleeing;
            }
            else
            {
                movementType = MovementType.Roaming;
                chaseTarget = null;
            }
            rb.AddForce(transform.forward * currentSpeed);
            rb.drag = Mathf.Abs(rb.velocity.z * dragMultiplier);
            //rb.velocity = transform.forward * currentSpeed + Vector3.down;
            //rb.velocity += Vector3.down * 2f;

            destinationPoint.y = transform.position.y;

            Vector3 targetDirection = destinationPoint - transform.position;
            targetDirection.y = 0;
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
                case MovementType.Fleeing:
                    currentSpeed = walkSpeed;
                    transform.rotation = Quaternion.LookRotation(newDirection);
                    break;
                default:
                    break;
            }

            if (movementType == MovementType.Roaming)
            {
                if (Vector3.Distance(transform.position, destinationPoint) <= destinationStoppingThreshold)
                {
                    idleTime = Random.Range(minIdleTime, maxIdleTime);
                    movementType = MovementType.Idle;
                    destinationPoint = Vector3.zero;
                }
            }
            if(movementType == MovementType.Fleeing)
            {
                if (transform.position.x <= trainScript.gameObject.transform.position.x) { destinationPoint = new Vector3(transform.position.x - fleeDistance,transform.position.y,transform.position.z); }
                if (transform.position.x > trainScript.gameObject.transform.position.x) { destinationPoint = new Vector3(transform.position.x + fleeDistance, transform.position.y, transform.position.z); }

            }
            else
            {
                if (chaseTarget != null && Vector3.Distance(transform.position, chaseTarget.position) <= destinationStoppingThreshold)
                {
                    // attack
                    attackTime -= Time.deltaTime;
                    if (attackTime <= 0)
                    {
                        attackTime = attackDelay;
                        //chaseTarget.GetComponent<Player>().ChangeHealth(attackDamage);
                        if (!game.enemyBuff)//This has been slightley changed as an example of the buff in the game script, can easly be changed back if needed
                        {
                            chaseTarget.GetComponent<Player>().ChangeHealth(attackDamage);
                        }
                        if(game.enemyBuff)
                        {
                            chaseTarget.GetComponent<Player>().ChangeHealth(attackDamage += attackBuff);
                        }
                    }
                }
            }
        }
        else if (movementType == MovementType.Idle)
        {
            rb.drag = 20;
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
        destinationPoint = new Vector3(transform.position.x + Random.Range(-roamingDistance, roamingDistance), transform.position.y, transform.position.z + Random.Range(-roamingDistance, roamingDistance));
        
        movementType = MovementType.Roaming;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(destinationPoint, 2);
    }
}
