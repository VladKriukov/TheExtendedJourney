using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float enemyHealth = 100f;

    public void TakeDamage(float damage)
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0)
        {
            GetComponent<Animator>().SetTrigger("Die");
            GetComponent<Enemy>().isAlive = false;
        }
    }
}