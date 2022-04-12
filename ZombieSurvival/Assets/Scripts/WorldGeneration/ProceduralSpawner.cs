using UnityEngine;
using System.Collections.Generic;

public class ProceduralSpawner : MonoBehaviour
{
    [Tooltip("The items to spawn per chunk")]
    [SerializeField] GameObject[] itemsToSpawn;
    [SerializeField] int minNumberToSpawn = 1;
    [SerializeField] int maxNumberToSpawn = 1;
    [SerializeField] float yOffset;
    [SerializeField] Vector2 minOffset;
    [SerializeField] Vector2 maxOffset;
    [SerializeField] bool changeScale;
    [SerializeField] float minScale = 1;
    [SerializeField] float maxScale = 1;
    [SerializeField] bool randomRotation;

    [Tooltip("The 1 in x chance of these items spawning in this chunk. 1 for 100%")]
    public int spawningChance = 1;

    List<GameObject> pool = new List<GameObject>();

    int rand;
    int rand2;

    private void Awake()
    {
        for (int i = 0; i < maxNumberToSpawn; i++)
        {
            pool.Add(Instantiate(itemsToSpawn[Random.Range(0, itemsToSpawn.Length - 1)], transform));
            pool[i].SetActive(false);
        }
    }

    public void Spawn()
    {
        rand = Random.Range(0, spawningChance);
        //Debug.Log(rand);
        if (rand > 1) return;

        rand = Random.Range(minNumberToSpawn, maxNumberToSpawn);
        for (int i = 0; i < rand; i++)
        {
            rand2 = Random.Range(0, pool.Count - 1);
            pool[rand2].SetActive(true);
            pool[rand2].transform.position = new Vector3(Random.Range(transform.position.x + minOffset.x, transform.position.x +  maxOffset.x), yOffset, Random.Range(transform.position.z + minOffset.y, transform.position.z + maxOffset.y));
            if (changeScale == true)
            {
                pool[rand2].transform.localScale = new Vector3(Random.Range(minScale, maxScale), Random.Range(minScale, maxScale), Random.Range(minScale, maxScale));
            }
            if (randomRotation == true)
            {
                pool[rand2].transform.Rotate(new Vector3(0, Random.Range(-180, 180), 0), Space.World);
            }
        }
    }
}