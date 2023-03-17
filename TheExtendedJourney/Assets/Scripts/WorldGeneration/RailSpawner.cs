using UnityEngine;

public class RailSpawner : MonoBehaviour
{
    [SerializeField] private GameObject connectedSpawner;

    [Tooltip("The chance of 1 in x that this rail will be a slope. The bigger the number, the lower the chance")]
    [SerializeField] private int slopeChance = 5;

    [SerializeField] private GameObject railStraight;
    [SerializeField] private GameObject railSlope;

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

    private GameObject instantiatedSpawner;

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

    private void SpawnSpawners()
    {
        thisChunkGenerator.connectedSpawners[0].GenerateTerrain(chunkSpawner.chunkAltitude, 1, true, startingSlopeType);
        thisChunkGenerator.connectedSpawners[1].GenerateTerrain(chunkSpawner.chunkAltitude, 1, false, startingSlopeType);
    }

    public void GenerateNextChunk()
    {
        finishedChunkCount++;
        if (finishedChunkCount == 2)
        {
            Debug.Log("Next chunk");
            //HideAllTerrains();
            chunkSpawner.GenerateNextChunk();
            finishedChunkCount = 0;
        }
    }

    public void ReGenerateRails(int chunkIndex)
    {
        EnableRails();
        HideAllTerrains();
        foreach (var item in thisChunkGenerator.connectedSpawners)
        {
            item.ReGenerateChunk(chunkIndex);
        }
    }

    void HideAllTerrains()
    {
        foreach (var item in thisChunkGenerator.connectedSpawners)
        {
            item.HideAllTerrains();
        }
    }

    private void GenerateRails(int previousAltitude)
    {
        if (firstTimeSpawning) PoolRails();

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
            }
        }
        else
        {
            startingSlopeType = StartingSlopeType.Straight;
        }

        if (previousAltitude < (Game.minAltitudeSteps + Game.waterLevel))
        {
            Debug.Log("prev altitude: " + previousAltitude + ", min altitude steps: " + Game.minAltitudeSteps + ", water level: " + (Game.waterLevel) + ", min alt steps + water level = " + (Game.minAltitudeSteps + Game.waterLevel - 1));
            startingSlopeType = StartingSlopeType.Up;
        }

        EnableRails();
        
        chunkSpawner.SpawnedRails();
    }

    void EnableRails()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);

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

        transform.GetChild(0).GetComponent<BiomeChanger>().ChangeBiome();
        transform.GetChild(1).GetComponent<BiomeChanger>().ChangeBiome();
    }

    private void PoolRails()
    {
        Instantiate(railStraight, transform).SetActive(false);
        Instantiate(railSlope, transform).SetActive(false);
    }
}