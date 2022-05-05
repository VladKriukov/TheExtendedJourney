using UnityEngine;
using System.Collections.Generic;

public class Resource : MonoBehaviour
{
    public enum EffectiveItem
    {
        NA,
        Rocks,
        Trees,
        Enemies
    }
    public EffectiveItem effectiveItem;
    [SerializeField] bool damagable;
    [SerializeField] float health;
    [SerializeField] List<GameObject> drops = new List<GameObject>();
    [SerializeField] int numberOfDrops;
    [SerializeField] Vector3 spawnRange;
    [SerializeField] Vector3 spawnOffset;
    [SerializeField] Quaternion rotation;

    public void Damage(float amount)
    {
        if (damagable == false) return;
        health -= amount;
        if (health <= 0)
        {
            for (int i = 0; i < numberOfDrops; i++)
            {
                Instantiate(drops[Random.Range(0, drops.Count)], transform.position + new Vector3(Random.Range(-spawnRange.x, spawnRange.x) + spawnOffset.x, Random.Range(-spawnRange.y, spawnRange.y) + spawnOffset.y, Random.Range(-spawnRange.z, spawnRange.z) + spawnOffset.z), rotation);
            }
            gameObject.SetActive(false);
        }
    }
}
