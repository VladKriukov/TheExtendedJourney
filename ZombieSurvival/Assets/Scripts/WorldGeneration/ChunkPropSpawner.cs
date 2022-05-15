using UnityEngine;
using System.Collections.Generic;

public class ChunkPropSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> itemVariants = new List<GameObject>();
    [SerializeField] int minAmount;
    [SerializeField] int maxAmount;
    [Tooltip("False = 100% chance of spawning")][SerializeField] bool randomiseSpawningChance;
    [SerializeField] float spawningChance;
    [SerializeField] Vector2 spawningRange;
    [SerializeField] Vector2 spawningOffset;
    [SerializeField] LayerMask layerMask;

    List<GameObject> spawnedItems = new List<GameObject>();

    GameObject item;
    RaycastHit hit;
    bool hitting;

    private void Awake()
    {
        minAmount = Mathf.Clamp(minAmount * Game.chunkPropMultiplier, 0, 15);
        maxAmount = Mathf.Clamp(maxAmount * Game.chunkPropMultiplier, 0, 25);
        spawningChance *= 1 / Game.chunkPropDensity;
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
            if (Random.Range(0, spawningChance) <= 1)
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
                if (item.GetComponent<ItemProperties>() != null)
                {
                    ItemProperties properties = item.GetComponent<ItemProperties>();
                    item.transform.position = new Vector3(item.transform.position.x + properties.spawningOffset.x, hit.point.y + properties.spawningOffset.y, item.transform.position.z + properties.spawningOffset.z);
                }
                else
                {
                    item.transform.position = new Vector3(item.transform.position.x, hit.point.y, item.transform.position.z);
                }
                
            }
            else
            {
                Debug.LogError("Failed to spawn");
                item.SetActive(false);
            }
        }
    }
}