using UnityEngine;
using System.Collections.Generic;

public class ChunkPropSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> itemVariants = new List<GameObject>();
    [SerializeField] float minAmount;
    [SerializeField] float maxAmount;
    [Tooltip("False = 100% chance of spawning")][SerializeField] bool randomiseSpawningChance;
    [SerializeField] int spawningChance;
    [SerializeField] Vector2 spawningRange;
    [SerializeField] Vector2 spawningOffset;
    [SerializeField] LayerMask layerMask;

    List<GameObject> spawnedItems = new List<GameObject>();

    GameObject item;
    RaycastHit hit;
    bool hitting;

    private void Awake()
    {
        PoolItems();
        CheckSpawning();
    }

    void PoolItems()
    {
        for (int i = 0; i < maxAmount; i++)
        {
            spawnedItems.Add(Instantiate(itemVariants[Random.Range(0, itemVariants.Count)], transform));
            spawnedItems[i].SetActive(false);
        }
    }

    void CheckSpawning()
    {
        if (randomiseSpawningChance == true)
        {
            if (Random.Range(0, spawningChance) == 0)
            {
                SpawnItems();
            }
        }
        else
        {
            SpawnItems();
        }
    }

    void SpawnItems()
    {
        for (int i = 0; i < Random.Range(minAmount, maxAmount); i++)
        {
            item = spawnedItems[i];
            item.SetActive(true);
            item.transform.position = new Vector3(transform.position.x + Random.Range(-spawningRange.x, spawningRange.x) + spawningOffset.x, transform.position.y + 10, transform.position.z + Random.Range(-spawningRange.y, spawningRange.y) + spawningOffset.y);
            hitting = Physics.Raycast(item.transform.position, Vector3.down, out hit, 50, layerMask);
            if (hitting == true)
            {
                item.transform.position = new Vector3(item.transform.position.x, hit.point.y, item.transform.position.z);
            }
            else
            {
                item.SetActive(false);
            }
        }
    }
}