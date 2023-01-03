using UnityEngine;
using System.Collections.Generic;

public class ChunkGenerator : MonoBehaviour
{
    [HideInInspector] public List<ConnectedSpawner> connectedSpawners = new List<ConnectedSpawner>();
    [HideInInspector] public RailSpawner railTerrainSpawner;

    public int chunkIndex;

    public delegate void MovingFromBehind();
    public static MovingFromBehind OnMovingFromBehind;

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
        switch (other.gameObject.tag)
        {
            case "ChunkForward":
                OnMovingFromBehind?.Invoke();
                chunkSpawner.GenerateNextChunk(transform);
                break;
            case "ChunkBackward":
                chunkSpawner.ReGenerateAChunk(this);
                break;
            default:
                break;
        }
    }
}