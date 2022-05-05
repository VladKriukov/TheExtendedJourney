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
        if (other.CompareTag("ChunkForward"))
        {
            chunkSpawner.GenerateNextChunk(chunkSpawner.GetChunk(chunkIndex).GetComponent<RailSpawner>().nextChunkAltitudeChange, transform);
        }
    }
}