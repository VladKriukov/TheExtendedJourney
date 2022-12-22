using UnityEngine;
using System.Collections.Generic;
using System;

public class ChunkSpawner : MonoBehaviour
{
    [SerializeField] GameObject chunk;
    [SerializeField] int numberOfChunksToSpawn;
    public float chunkYOffset; // the chunk height offset whenever the chunk altitude changes
    public float chunkXOffset; // the chunk size in width (possibly same as Z offset since it is square)
    public float chunkZOffset; // the chunk size in length

    [SerializeField] GameObject startingPlatform;

    [HideInInspector] public int chunkAltitude;
    
    public List<SavedChunk> savedChunks = new List<SavedChunk>();
    public SavedChunk currentChunk;

    List<ChunkGenerator> chunkGenerators = new List<ChunkGenerator>();
    
    int chunkDistanceCounter;
    ChunkGenerator chunkOfInterest;
    GameObject water;

    private void Awake()
    {
        numberOfChunksToSpawn = Game.numberOfChunksToSpawn;
        Game.totalProgressParts = numberOfChunksToSpawn * Game.maxChunksFromTrack * 2;
        chunkGenerators.Add(Instantiate(chunk, transform).GetComponent<ChunkGenerator>());
        chunkOfInterest = chunkGenerators[0];
        water = transform.parent.GetChild(1).gameObject;
        water.transform.position = new Vector3(0, (Game.minAltitudeSteps + Game.waterLevel - 1) * chunkYOffset, transform.position.z);
        currentChunk.chunkGenerator = chunkOfInterest;
        savedChunks.Add(currentChunk);
        //savedChunks[0] = currentChunk; // used to save the chunk data
    }

    public void GenerateNextChunk(Transform chunkToMove = null)
    {
        SaveMovingChunk(chunkOfInterest);
        if (chunkGenerators.Count >= numberOfChunksToSpawn) // if the chunks are being moved from behind
        {
            if (chunkToMove != null)
            {
                SaveMovingChunk(chunkGenerators[chunkGenerators.Count - 1]);
                Debug.Log("Attempting to move chunk from behind");
                SpawnChunk(chunkToMove);
                MoveChunk();
            }
            else
            {
                Debug.Log("Stopped Spawning");
                CancelInvoke();
            }
        }
        else
        {
            chunkGenerators.Add(Instantiate(chunk, transform).GetComponent<ChunkGenerator>());
            SpawnChunk(chunkGenerators[chunkGenerators.Count - 1].transform);
        }
    }

    void SaveMovingChunk(ChunkGenerator chunkToSave)
    {
        // Saves all the connected spawner infos for each connected spawner from that line (the X axis line, children of 1 chunk generator) of connected spawners. The info for each one would be then retrieved through the "chunkFromTrack" index within each connected spawner, taken from the savedChunks list
        // create new save (new item in list) if this hasn't been saved before, otherwise update it

        //ChunkGenerator chunkGenerator = chunkToSave.GetComponent<ChunkGenerator>();

        Debug.Log("current chunk index: " + chunkToSave.chunkIndex + ", number of saved chunks: " + savedChunks.Count);

        currentChunk.railSlope = chunkToSave.GetComponent<RailSpawner>().startingSlopeType;
        currentChunk.chunkHeight = chunkAltitude; //* chunkYOffset;
        currentChunk.chunkGenerator = chunkToSave;
        currentChunk.connectedSpawners.Clear();
        for (int i = 0; i < chunkToSave.connectedSpawners.Count; i++)
        {
            currentChunk.connectedSpawners.Add(chunkToSave.connectedSpawners[i].currentChunk);
        }

        if (savedChunks.Count < chunkToSave.chunkIndex + 1)
        {
            savedChunks.Add(currentChunk);
        }
        else
        {
            savedChunks[chunkToSave.chunkIndex] = currentChunk;
        }
    }

    void SpawnChunk(Transform chunkToSpawn)
    {
        chunkOfInterest = chunkToSpawn.GetComponent<ChunkGenerator>();

        chunkDistanceCounter++;
        chunkOfInterest.chunkIndex = chunkDistanceCounter;
    }

    void MoveChunk()
    {
        chunkOfInterest.transform.parent.GetComponent<ChunkSpawner>().chunkAltitude = chunkAltitude;

        chunkOfInterest.SpawnChunk();
        water.transform.position += new Vector3(0, 0, 30);
    }

    public void SpawnedRails()
    {
        RailSpawner railSpawner = chunkOfInterest.GetComponent<RailSpawner>();

        if (chunkGenerators.Count > 1)
        {
            RailSpawner previousRailSpawner = GetChunk(chunkOfInterest.chunkIndex - 1).GetComponent<RailSpawner>();

            // this used to be in the rail spawner but it works here because of the order of execution
            switch (railSpawner.startingSlopeType)
            {
                case RailSpawner.StartingSlopeType.Straight:
                    switch (previousRailSpawner.nextChunkAltitudeChange)
                    {
                        case 0:

                            break;
                        case 1:
                            chunkAltitude += 1;
                            break;
                        case -1:

                            break;
                        default:
                            break;
                    }
                    break;
                case RailSpawner.StartingSlopeType.Up:
                    switch (previousRailSpawner.nextChunkAltitudeChange)
                    {
                        case 0:

                            break;
                        case 1:
                            chunkAltitude += 1;
                            break;
                        case -1:

                            break;
                        default:
                            break;
                    }
                    break;
                case RailSpawner.StartingSlopeType.Down:
                    switch (previousRailSpawner.nextChunkAltitudeChange)
                    {
                        case 0:
                            chunkAltitude -= 1;
                            break;
                        case 1:

                            break;
                        case -1:
                            chunkAltitude -= 1;
                            break;
                        default:
                            break;
                    }
                    break;
                case RailSpawner.StartingSlopeType.NA:
                    break;
                default:
                    break;
            }
        }
        else
        {
            if (railSpawner.startingSlopeType == RailSpawner.StartingSlopeType.Down)
            {
                startingPlatform.transform.position = new Vector3(startingPlatform.transform.position.x, 6, startingPlatform.transform.position.z);
            }
        }
        
        chunkOfInterest.transform.position = new Vector3(0, chunkAltitude * chunkYOffset, chunkZOffset * chunkDistanceCounter);
    }

    public ChunkGenerator GetChunk(int number)
    {
        foreach (ChunkGenerator item in chunkGenerators)
        {
            if (number == item.chunkIndex)
            {
                return item;
            }
        }
        return null;
    }
}