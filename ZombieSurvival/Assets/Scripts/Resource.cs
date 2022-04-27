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
    public bool damagable;
    public float health;
    public List<GameObject> drops = new List<GameObject>();
    public int numberOfDrops;
    public Vector3 spawnRange;
    public Vector3 spawnOffset;

    public void Damage(float amount)
    {
        if (damagable == false) return;
        Debug.Log("Removing " + amount + " health");
        health -= amount;
        if (health <= 0)
        {
            for (int i = 0; i < numberOfDrops; i++)
            {
                Instantiate(drops[Random.Range(0, drops.Count)], transform.position + new Vector3(Random.Range(-spawnRange.x, spawnRange.x) + spawnOffset.x, Random.Range(-spawnRange.y, spawnRange.y) + spawnOffset.y, Random.Range(-spawnRange.z, spawnRange.z) + spawnOffset.z), new Quaternion(0, Random.Range(-180, 180), 0, 1));
            }
            gameObject.SetActive(false);
        }
    }
}
