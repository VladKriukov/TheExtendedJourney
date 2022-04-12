using UnityEngine;
using System.Collections.Generic;

public class ChunkGenerator : MonoBehaviour
{
    [HideInInspector] public List<ConnectedSpawner> connectedSpawners = new List<ConnectedSpawner>();
    [HideInInspector] public RailSpawner railTerrainSpawner;

    [HideInInspector] public int chunkIndex;

    ChunkSpawner chunkSpawner;

    private void Start()
    {
        railTerrainSpawner = GetComponent<RailSpawner>();
        chunkSpawner = transform.parent.GetComponent<ChunkSpawner>();

        SpawnChunk();
    }

    public void SpawnChunk()
    {
        railTerrainSpawner.GenerateRailTerrain(chunkSpawner.chunkAltitude);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered something");
        if (other.CompareTag("ChunkForward"))
        {
            chunkSpawner.GenerateNextChunk(chunkSpawner.GetChunk(chunkIndex - 1).GetComponent<RailSpawner>().nextChunkAltitudeChange, transform);
        }
    }
}