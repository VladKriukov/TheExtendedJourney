using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ChunkSpawner : MonoBehaviour
{
    [SerializeField] GameObject chunk;
    [SerializeField] int numberOfChunksToSpawn;
    public float chunkYOffset; // the chunk height offset whenever the chunk altitude changes
    public float chunkXOffset; // the chunk size in width (possibly same as Z offset since it is square)
    public float chunkZOffset; // the chunk size in length

    [HideInInspector] public int chunkAltitude;

    List<ChunkGenerator> chunkGenerators = new List<ChunkGenerator>();

    int chunkDistanceCounter;
    ChunkGenerator chunkOfInterest;

    private void Awake()
    {
        chunkGenerators.Add(Instantiate(chunk, transform).GetComponent<ChunkGenerator>());
        chunkOfInterest = chunkGenerators[0];
    }

    public void GenerateNextChunk(int nextChunkAltitude, Transform chunkToMove = null)
    {
        if (chunkGenerators.Count >= numberOfChunksToSpawn)
        {
            if (chunkToMove != null)
            {
                MoveChunk(chunkToMove, nextChunkAltitude);
                //StartCoroutine(nameof(MoveChunk), 0f);
            }
            else
            {
                Debug.Log("Stopped Spawning");
            }
        }
        else
        {
            chunkGenerators.Add(Instantiate(chunk, transform).GetComponent<ChunkGenerator>());
            MoveChunk(chunkGenerators[chunkGenerators.Count-1].transform, nextChunkAltitude);
            //StartCoroutine(nameof(MoveChunk), 0f);
        }
    }

    void MoveChunk(Transform chunkToMove, int nextChunkAltitude)
    {
        chunkOfInterest = chunkToMove.GetComponent<ChunkGenerator>();

        //Debug.Log("=====================================");
        //Debug.Log("Next chunk altitude: " + nextChunkAltitude);
        //Debug.Log("Previous chunk count: " + (chunkGenerators.Count - 2));
        //Debug.Log("Previous chunk index: " + chunkGenerators[chunkGenerators.Count - 2].chunkIndex);

        Invoke(nameof(Spawn), 0.5f);
        //Spawn();

        //Invoke(nameof(Move), 1f);
        //chunkToMove.position = new Vector3(0, chunkAltitude * chunkYOffset, chunkZOffset * (chunkDistanceCounter + 1));

        chunkDistanceCounter++;
        chunkOfInterest.chunkIndex = chunkDistanceCounter;
        Debug.Log("Chunk number: " + chunkDistanceCounter);
    }

    public void SpawnedRails()
    {
        RailSpawner railSpawner = chunkOfInterest.GetComponent<RailSpawner>();

        if (chunkGenerators.Count > 1)
        {
            RailSpawner previousRailSpawner = chunkGenerators[chunkGenerators.Count - 2].GetComponent<RailSpawner>();

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
        
        chunkOfInterest.transform.position = new Vector3(0, chunkAltitude * chunkYOffset, chunkZOffset * chunkDistanceCounter);
    }

    void Spawn()
    {
        if (chunkGenerators.Count >= numberOfChunksToSpawn)
        {
            chunkOfInterest.SpawnChunk();
        }
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