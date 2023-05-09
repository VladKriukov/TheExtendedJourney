using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SavedChunk
{
    public RailSpawner.StartingSlopeType railSlope;
    public float chunkHeight;
    public ChunkGenerator chunkGenerator;
    public ChunkSpawner.CurrentBiome chunkBiome;
    public List<ConnectedSpawnerInfo> connectedSpawners;
}

[Serializable]
public struct ConnectedSpawnerInfo
{
    public GameObject spawnedChunk;
    public bool spawningDirection;
    public int chunkFromTrack;
    public Vector3 position;
    public List<ChunkPropInfo> chunkProps;
    public GameObject structure; // todo: add structures
}

[Serializable]
public struct ChunkPropInfo
{
    public GameObject chunkPropRef;
    public GameObject chunkPropPrefab;
}