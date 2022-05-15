using UnityEngine;

public class RailSpawner : MonoBehaviour
{
    [SerializeField] GameObject connectedSpawner;
    [Tooltip("The chance of 1 in x that this rail will be a slope. The bigger the number, the lower the chance")]
    [SerializeField] int slopeChance = 5;
    [SerializeField] GameObject railStraight;
    [SerializeField] GameObject railSlope;

    [HideInInspector]
    public enum StartingSlopeType
    {
        Straight, Up, Down, NA
    }
    [HideInInspector] public StartingSlopeType startingSlopeType;

    [HideInInspector] public int nextChunkAltitudeChange;

    private int rand;
    private bool firstTimeSpawning = true;
    private ChunkGenerator thisChunkGenerator;
    private ChunkSpawner chunkSpawner;
    private int finishedChunkCount;

    GameObject instantiatedSpawner;

    private void Awake()
    {
        thisChunkGenerator = GetComponent<ChunkGenerator>();
        chunkSpawner = transform.parent.GetComponent<ChunkSpawner>();
        slopeChance = Game.railFlatness;
    }

    public void GenerateRailTerrain(int altitude)
    {
        GenerateRails(altitude);
        if (firstTimeSpawning)
        {
            firstTimeSpawning = false;

            instantiatedSpawner = Instantiate(connectedSpawner, new Vector3(transform.position.x + chunkSpawner.chunkXOffset, transform.position.y, transform.position.z), Quaternion.identity, transform);

            thisChunkGenerator.connectedSpawners.Add(instantiatedSpawner.GetComponent<ConnectedSpawner>());

            instantiatedSpawner = Instantiate(connectedSpawner, new Vector3(transform.position.x - chunkSpawner.chunkXOffset, transform.position.y, transform.position.z), Quaternion.identity, transform);
            thisChunkGenerator.connectedSpawners.Add(instantiatedSpawner.GetComponent<ConnectedSpawner>());
        }
        Invoke(nameof(SpawnSpawners), 0.1f);
        return;
    }

    void SpawnSpawners()
    {
        thisChunkGenerator.connectedSpawners[0].GenerateTerrain(chunkSpawner.chunkAltitude, 1, true, startingSlopeType);
        thisChunkGenerator.connectedSpawners[1].GenerateTerrain(chunkSpawner.chunkAltitude, 1, false, startingSlopeType);
    }

    public void GenerateNextChunk()
    {
        finishedChunkCount++;
        if (finishedChunkCount == 2)
        {
            chunkSpawner.GenerateNextChunk(nextChunkAltitudeChange);
            finishedChunkCount = 0;
        }
    }

    void GenerateRails(int previousAltitude)
    {
        if (firstTimeSpawning) PoolRails();

        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);

        rand = Random.Range(0, slopeChance);
        if (rand <= 1)
        {
            if (previousAltitude < Game.maxAltitudeSteps)
            {
                rand = Random.Range(1, 3);
                startingSlopeType = (StartingSlopeType)rand;
            }
            else
            {
                startingSlopeType = StartingSlopeType.Down;

                if (previousAltitude > Game.minAltitudeSteps)
                {
                    rand = Random.Range(1, 3);
                    startingSlopeType = (StartingSlopeType)rand;
                }
                else
                {
                    startingSlopeType = StartingSlopeType.Up;
                }
            }
        }
        else
        {
            startingSlopeType = StartingSlopeType.Straight;
        }

        switch (startingSlopeType)
        {
            case StartingSlopeType.Straight:
                transform.GetChild(0).gameObject.SetActive(true);
                nextChunkAltitudeChange = 0;
                break;
            case StartingSlopeType.Up:
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(1).eulerAngles = new Vector3(0, 180, 0);
                nextChunkAltitudeChange = 1;
                break;
            case StartingSlopeType.Down:
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(1).eulerAngles = new Vector3(0, 0, 0);
                nextChunkAltitudeChange = -1;
                break;
            default:
                break;
        }
        chunkSpawner.SpawnedRails();
    }

    void PoolRails()
    {
        Instantiate(railStraight, transform).SetActive(false);
        Instantiate(railSlope, transform).SetActive(false);
    }
}