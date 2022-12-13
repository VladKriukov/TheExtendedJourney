using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SavedChunk
{
    public RailSpawner.StartingSlopeType railSlope;
    public float chunkHeight;
    public ChunkGenerator chunkGenerator;
    public List<ConnectedSpawnerInfo> connectedSpawners;
}

[Serializable]
public struct ConnectedSpawnerInfo
{
    public GameObject spawnedChunk;
    public List<GameObject> chunkProps;
    public GameObject structure; // todo: add structures
}