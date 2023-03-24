using UnityEngine;
using System.Collections.Generic;

public class ChunkPropSpawner : MonoBehaviour
{
    [SerializeField] ItemVariants itemVariants;
    //[SerializeField] List<GameObject> seasonalChristmasItems = new List<GameObject>();
    [SerializeField] int minAmount;
    [SerializeField] int maxAmount;
    [Tooltip("False = 100% chance of spawning")][SerializeField] bool randomiseSpawningChance;
    [SerializeField] float spawningChance;
    [SerializeField] Vector2 spawningRange;
    [SerializeField] Vector2 spawningOffset;
    [SerializeField] bool spawnAtY0;
    [SerializeField] LayerMask layerMask;

    List<GameObject> currentItemVariants = new List<GameObject>();
    List<GameObject> spawnedGrassItems = new List<GameObject>();
    List<GameObject> spawnedSnowItems = new List<GameObject>();
    List<GameObject> spawnedSandItems = new List<GameObject>();
    List<GameObject> spawnedFallItems = new List<GameObject>();

    List<GameObject> spawnedItems = new List<GameObject>();

    List<ChunkPropInfo> allChunkPropInfos = new List<ChunkPropInfo>();
    List<ChunkPropInfo> activeChunkPropInfos = new List<ChunkPropInfo>();

    // seasonal enum

    GameObject item;
    RaycastHit hit;
    bool hitting;

    private void Awake()
    {
        minAmount = Mathf.Clamp(minAmount * Game.chunkPropMultiplier, 0, 15);
        maxAmount = Mathf.Clamp(maxAmount * Game.chunkPropMultiplier, 0, 25);
        spawningChance *= 1 / Game.chunkPropDensity;
    }

    private void OnEnable()
    {
        PoolItems();
        CheckSpawning();
    }

    private void OnDisable()
    {
        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(false);
        }
    }

    void PoolItems()
    {
        ChunkPropInfo chunkPropInfo = new ChunkPropInfo();

        spawnedGrassItems = InstantiateItems(spawnedGrassItems, itemVariants.itemVariants_Normal, chunkPropInfo);
        spawnedSnowItems = InstantiateItems(spawnedSnowItems, itemVariants.itemVariants_Snow, chunkPropInfo);
        spawnedSandItems = InstantiateItems(spawnedSandItems, itemVariants.itemVariants_Sand, chunkPropInfo);
        spawnedFallItems = InstantiateItems(spawnedFallItems, itemVariants.itemVariants_Fall, chunkPropInfo);

        switch (ChunkSpawner.currentBiome)
        {
            case ChunkSpawner.CurrentBiome.Grass:
                currentItemVariants = itemVariants.itemVariants_Normal;
                spawnedItems = spawnedGrassItems;
                Debug.LogWarning("Grass");
                break;
            case ChunkSpawner.CurrentBiome.Snow:
                currentItemVariants = itemVariants.itemVariants_Snow;
                spawnedItems = spawnedSnowItems;
                Debug.LogWarning("Snow");
                break;
            case ChunkSpawner.CurrentBiome.Sand:
                currentItemVariants = itemVariants.itemVariants_Sand;
                spawnedItems = spawnedSandItems;
                Debug.LogWarning("Sand");
                break;
            case ChunkSpawner.CurrentBiome.Fall:
                currentItemVariants = itemVariants.itemVariants_Fall;
                spawnedItems = spawnedFallItems;
                Debug.LogWarning("Fall");
                break;
            default:
                break;
        }

        //if (currentItemVariants.Count == 0) return;

        
        /*for (int i = 0; i < maxAmount; i++)
        {
            if (spawnedItems.Count >= maxAmount) break;

            int rand = Random.Range(0, currentItemVariants.Count);
            spawnedItems.Add(Instantiate(currentItemVariants[rand], transform));
            spawnedItems[i].SetActive(false);

            chunkPropInfo.chunkPropRef = spawnedItems[i];
            chunkPropInfo.chunkPropPrefab = currentItemVariants[rand];
            allChunkPropInfos.Add(chunkPropInfo);
        }
        */
    }

    List<GameObject> InstantiateItems(List<GameObject> spawnedItems, List<GameObject> itemVariants, ChunkPropInfo chunkPropInfo)
    {
        if (itemVariants.Count == 0) return null;

        for (int i = 0; i < maxAmount; i++)
        {
            if (spawnedItems.Count >= maxAmount) break;
            int rand = Random.Range(0, itemVariants.Count);

            spawnedItems.Add(Instantiate(itemVariants[rand], transform));
            spawnedItems[i].SetActive(false);

            chunkPropInfo.chunkPropRef = spawnedItems[i];
            chunkPropInfo.chunkPropPrefab = itemVariants[rand];
            allChunkPropInfos.Add(chunkPropInfo);
        }
        return spawnedItems;
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
        if (currentItemVariants.Count == 0) return;
        for (int i = 0; i < Random.Range(minAmount, maxAmount); i++)
        {
            if (transform.parent.position.y <= (-Game.waterLevel - 1) * 6)
            {
                break;
            }
            item = spawnedItems[i];
            item.SetActive(true);
            item.transform.position = new Vector3(transform.position.x + Random.Range(-spawningRange.x, spawningRange.x) + spawningOffset.x, transform.position.y + 100, transform.position.z + Random.Range(-spawningRange.y, spawningRange.y) + spawningOffset.y);
            hitting = Physics.Raycast(item.transform.position, Vector3.down, out hit, 200, layerMask);
            if (hitting == true)
            {
                if (hit.collider.gameObject.CompareTag("Water"))
                {
                    item.SetActive(false);
                    Debug.Log("HIT WATER");
                    break;
                }
                if (item.GetComponent<ItemProperties>() != null)
                {
                    ItemProperties properties = item.GetComponent<ItemProperties>();
                    item.transform.position = new Vector3(item.transform.position.x + properties.spawningOffset.x, hit.point.y + properties.spawningOffset.y, item.transform.position.z + properties.spawningOffset.z);
                }
                else
                {
                    item.transform.position = new Vector3(item.transform.position.x, hit.point.y, item.transform.position.z);
                }
                if (spawnAtY0 == true)
                {
                    item.transform.position = new Vector3(item.transform.position.x, transform.position.y, item.transform.position.z);
                }
            }
            else
            {
                Debug.LogError("Failed to spawn");
                item.SetActive(false);
            }
            if (item.activeInHierarchy == true)
            {
                activeChunkPropInfos.Add(allChunkPropInfos[i]);
            }
        }
        Invoke(nameof(SaveSpawnedItems), 0.5f);
    }

    void SaveSpawnedItems()
    {
        foreach (var item in activeChunkPropInfos)
        {
            if (item.chunkPropRef.activeInHierarchy == true)
            {
                if (transform.parent.GetComponent<ConnectedSpawner>() != null)
                {
                    transform.parent.GetComponent<ConnectedSpawner>().currentChunk.chunkProps.Add(item);
                }
            }
        }
    }
}