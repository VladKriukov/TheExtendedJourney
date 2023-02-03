using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] Transform aiTarget;
    [SerializeField] float viewRange = 5f;
    [SerializeField] float attackDamage = 20f;

    [HideInInspector] public bool canAttack;
    [HideInInspector] public bool isAlive = true;

    private float distanceToTarget;

    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        aiTarget = FindObjectOfType<PlayerHealth>().transform;
    }

    private void Update()
    {
        if (isAlive == true)
        {
            distanceToTarget = Vector3.Distance(aiTarget.position, transform.position);
            if (canAttack)
            {
                EngagePlayer();
            }
            else if (distanceToTarget <= viewRange)
            {
                canAttack = true;
            }
        }
        else
        {
            agent.isStopped = true;
        }
    }

    private void EngagePlayer()
    {
        if (distanceToTarget >= agent.stoppingDistance)
        {
            ChaseAITarget();
        }

        if (distanceToTarget <= agent.stoppingDistance)
        {
            AttackAITarget();
        }
    }

    private void ChaseAITarget()
    {
        if (distanceToTarget <= viewRange)
        {
            agent.SetDestination(aiTarget.position);
            //GetComponent<Animator>().SetTrigger("Move");
            //GetComponent<Animator>().SetBool("Attack", false);
        }
    }

    private void AttackAITarget()
    {
        //GetComponent<Animator>().SetBool("Attack", true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRange);
    }

    public void DamageThePlayer()
    {
        if (aiTarget == null) return;
        //aiTarget.GetComponent<PlayerHealth>().Damage(attackDamage);
    }
}
