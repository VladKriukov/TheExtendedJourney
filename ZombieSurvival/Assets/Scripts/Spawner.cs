using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [SerializeField] float maxSpawnTime = 3f;
    [SerializeField] float randomDistance = 5;
    float spawnTimer;

    [SerializeField] GameObject enemy;

    List<Transform> enemySpawns = new List<Transform>();

    private void Awake()
    {
        foreach (Transform item in transform)
        {
            enemySpawns.Add(item);
        }
        spawnTimer = maxSpawnTime;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            int rand = Random.Range(0, enemySpawns.Count);
            float randOffset = Random.Range(- randomDistance, randomDistance);
            GameObject enemyInstance = Instantiate(enemy, new Vector3(enemySpawns[rand].position.x + randomDistance, enemySpawns[rand].position.y, enemySpawns[rand].position.z + randomDistance), enemySpawns[rand].rotation);
            spawnTimer = maxSpawnTime;
        }
    }
}