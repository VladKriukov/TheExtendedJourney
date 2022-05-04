using UnityEngine;
using System.Collections.Generic;

public class ConnectedSpawner : MonoBehaviour
{
    [SerializeField] GameObject connectedSpawner;
    [SerializeField] float connectedSpawnerOffset;

    public List<GameObject> spawningTerrains = new List<GameObject>(); // GameObjects to instantiate

    [HideInInspector] public PossibleChunks generatedTerrain; // the terrain that was selected to generate for this spawner

    [HideInInspector] public ConnectedSpawner prevXSpawner;
    [HideInInspector] public ConnectedSpawner prevZSpawner;

    [HideInInspector] public int currentAltitude;
    [HideInInspector] public int currentChunkFromTrack;
    [HideInInspector] public bool spawningDirection;

    private ConnectedSpawner generatedConnectedSpawner;

    private List<GameObject> leftStraightItems = new List<GameObject>();
    private List<GameObject> rightStraightItems = new List<GameObject>();
    private List<GameObject> leftUpItems = new List<GameObject>();
    private List<GameObject> rightUpItems = new List<GameObject>();
    private List<GameObject> leftDownItems = new List<GameObject>();
    private List<GameObject> rightDownItems = new List<GameObject>();

    private List<GameObject> possibleZItems = new List<GameObject>();
    private List<GameObject> possibleXItems = new List<GameObject>();
    private List<GameObject> possibleXItemsBeforeChange = new List<GameObject>();
    private List<GameObject> possibleXSlopes = new List<GameObject>();
    private List<GameObject> possibleChunksToSpawn = new List<GameObject>();

    private int rand;
    private ChunkGenerator thisChunkGenerator;
    private ChunkSpawner chunkSpawner;
    private bool firstTimeSpawning = true;
    private bool nextSpawnerExists;
    private bool foundItem;
    private List<int> possibleSelectedTerrainIndexes = new List<int>();
    
    private void Awake()
    {
        chunkSpawner = transform.parent.parent.GetComponent<ChunkSpawner>();
        thisChunkGenerator = transform.parent.GetComponent<ChunkGenerator>();
    }

    public void GenerateTerrain(int _currentAltitudeStep, int _currentChunkFromTrack, bool rightDirection, RailSpawner.StartingSlopeType slopeType = RailSpawner.StartingSlopeType.NA)
    {
        currentAltitude = _currentAltitudeStep;
        currentChunkFromTrack = _currentChunkFromTrack;
        spawningDirection = rightDirection;

        possibleXItems.Clear();
        possibleZItems.Clear();
        possibleChunksToSpawn.Clear();
        possibleSelectedTerrainIndexes.Clear();
        prevXSpawner = null;
        prevZSpawner = null;
        generatedConnectedSpawner = null;
        nextSpawnerExists = false;

        if (firstTimeSpawning)
        {
            PoolTerrains();
        }

        // get previousZ chunk here
        for (int i = 0; i < thisChunkGenerator.connectedSpawners.Count; i++)
        {
            if (thisChunkGenerator.chunkIndex == 0) break; // break out of the loop since this is the first chunk
            // todo: fix later for backwards spawning

            ConnectedSpawner backwardsChunk = chunkSpawner.GetChunk(thisChunkGenerator.chunkIndex - 1).connectedSpawners[i];
            if (backwardsChunk.currentChunkFromTrack == _currentChunkFromTrack && backwardsChunk.spawningDirection == spawningDirection)
            {
                prevZSpawner = backwardsChunk;
                break;
            }
            
        }
        
        switch (slopeType)
        {
            case RailSpawner.StartingSlopeType.Straight:
                if (spawningDirection == false)
                {
                    SetFirstGeneratedTerrain(leftStraightItems);
                    transform.position = new Vector3(transform.position.x, transform.position.y + generatedTerrain.leftStartingStep * chunkSpawner.chunkYOffset, transform.position.z);
                }
                else
                {
                    SetFirstGeneratedTerrain(rightStraightItems);
                    transform.position = new Vector3(transform.position.x, transform.position.y + generatedTerrain.rightStartingStep * chunkSpawner.chunkYOffset, transform.position.z);
                }
                break;
            case RailSpawner.StartingSlopeType.Up:
                if (spawningDirection == false)
                {
                    SetFirstGeneratedTerrain(leftUpItems);
                    transform.position = new Vector3(transform.position.x, transform.position.y + generatedTerrain.leftStartingStep * chunkSpawner.chunkYOffset, transform.position.z);
                }
                else
                {
                    SetFirstGeneratedTerrain(rightUpItems);
                    transform.position = new Vector3(transform.position.x, transform.position.y + generatedTerrain.rightStartingStep * chunkSpawner.chunkYOffset, transform.position.z);
                }
                break;
            case RailSpawner.StartingSlopeType.Down:
                if (spawningDirection == false)
                {
                    SetFirstGeneratedTerrain(leftDownItems);
                    transform.position = new Vector3(transform.position.x, transform.position.y + generatedTerrain.leftStartingStep * chunkSpawner.chunkYOffset, transform.position.z);
                }
                else
                {
                    SetFirstGeneratedTerrain(rightDownItems);
                    transform.position = new Vector3(transform.position.x, transform.position.y + generatedTerrain.rightStartingStep * chunkSpawner.chunkYOffset, transform.position.z);
                }
                break;
            case RailSpawner.StartingSlopeType.NA:
                // get the previous X chunk since this is not the first chunk to spawn from the rails
                prevXSpawner = GetThisChunkSpawner(currentChunkFromTrack - 1);

                if (spawningDirection == true)
                {
                    switch (prevXSpawner.generatedTerrain.leftSlopeMatch)
                    {
                        case PossibleChunks.LeftStartingSlopeType.Straight:
                            possibleXItems = rightStraightItems;
                            break;
                        case PossibleChunks.LeftStartingSlopeType.Up:
                            possibleXItems = rightUpItems;
                            break;
                        case PossibleChunks.LeftStartingSlopeType.Down:
                            possibleXItems = rightDownItems;
                            break;
                        case PossibleChunks.LeftStartingSlopeType.NA:
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (prevXSpawner.generatedTerrain.rightSlopeMatch)
                    {
                        case PossibleChunks.RightStartingSlopeType.Straight:
                            possibleXItems = leftStraightItems;
                            break;
                        case PossibleChunks.RightStartingSlopeType.Up:
                            possibleXItems = leftUpItems;
                            break;
                        case PossibleChunks.RightStartingSlopeType.Down:
                            possibleXItems = leftDownItems;
                            break;
                        case PossibleChunks.RightStartingSlopeType.NA:
                            break;
                        default:
                            break;
                    }
                }
                
                if (prevZSpawner != null)
                {
                    CollectAndComparePossibleTerrainsToSpawn();
                }
                else
                {
                    SelectXItems();
                }
                
                rand = Random.Range(0, possibleChunksToSpawn.Count);

                SelectItem();

                // if current chunk needs to change the height in the direction of the previous chunk for the previous chunk, then change height
                if (spawningDirection == false)
                {
                    // get previous chunk and find it in the list of own possible chunks from that direction
                    for (int i = 0; i < prevXSpawner.generatedTerrain.rightItems.Length; i++)
                    {
                        if (generatedTerrain.name.Contains(prevXSpawner.generatedTerrain.rightItems[i].name))
                        {
                            Debug.Log("For terrain " + generatedTerrain.name + ", terrain altitude index of the previous terrain (" + prevXSpawner.generatedTerrain.rightItems[i].name + ") is " + i);
                            transform.position = new Vector3(transform.position.x, transform.position.y + prevXSpawner.generatedTerrain.requiredRightAltitudeSteps[i] * chunkSpawner.chunkYOffset, transform.position.z);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < prevXSpawner.generatedTerrain.leftItems.Length; i++)
                    {
                        if (generatedTerrain.name.Contains(prevXSpawner.generatedTerrain.leftItems[i].name))
                        {
                            Debug.Log("For terrain " + generatedTerrain.name + ", terrain altitude index of the previous terrain (" + prevXSpawner.generatedTerrain.leftItems[i].name + ") is " + i);
                            transform.position = new Vector3(transform.position.x, transform.position.y + prevXSpawner.generatedTerrain.requiredLeftAltitudeSteps[i] * chunkSpawner.chunkYOffset, transform.position.z);
                        }
                    }
                }
                break;
            default:
                break;
        }

        generatedTerrain.gameObject.SetActive(true);

        // check if there is a need to spawn another connected spawner
        if (GetThisChunkSpawner(currentChunkFromTrack + 1) != null)
        {
            nextSpawnerExists = true;
        }

        if (currentChunkFromTrack < Game.maxChunksFromTrack)
        {
            if (spawningDirection == false)
            {
                if (nextSpawnerExists == false)
                {
                    thisChunkGenerator.connectedSpawners.Add(Instantiate(connectedSpawner, new Vector3(transform.position.x - chunkSpawner.chunkXOffset, transform.position.y, transform.position.z), Quaternion.identity, transform.parent).GetComponent<ConnectedSpawner>());
                    generatedConnectedSpawner = thisChunkGenerator.connectedSpawners[thisChunkGenerator.connectedSpawners.Count - 1];
                }
                if (generatedConnectedSpawner != null)
                {
                    generatedConnectedSpawner.GenerateTerrain(generatedTerrain.requiredLeftAltitudeSteps[possibleSelectedTerrainIndexes[rand]], currentChunkFromTrack + 1, spawningDirection);
                }
            }
            else
            {
                if (nextSpawnerExists == false)
                {
                    thisChunkGenerator.connectedSpawners.Add(Instantiate(connectedSpawner, new Vector3(transform.position.x + chunkSpawner.chunkXOffset, transform.position.y, transform.position.z), Quaternion.identity, transform.parent).GetComponent<ConnectedSpawner>());
                    generatedConnectedSpawner = thisChunkGenerator.connectedSpawners[thisChunkGenerator.connectedSpawners.Count - 1];
                }

                if (generatedConnectedSpawner != null)
                {
                    Debug.Log("required steps length: " + generatedTerrain.requiredRightAltitudeSteps.Length + ", possible indexes length: " + possibleSelectedTerrainIndexes.Count + ", rand: " + rand + ", randth index: " + possibleSelectedTerrainIndexes[rand] + ", generated terrain: " + generatedTerrain.name + ", possibleXItems length: " + possibleXItems.Count);
                    generatedConnectedSpawner.GenerateTerrain(generatedTerrain.requiredRightAltitudeSteps[possibleSelectedTerrainIndexes[rand]], currentChunkFromTrack + 1, spawningDirection);
                }
            }
        }
        else
        {
            Invoke(nameof(GenerateNextBigChunk), 0.5f);
        }
    }

    void CollectAndComparePossibleTerrainsToSpawn()
    {
        // collect all the possible combinations in the vertical direction
        foreach (var item in prevZSpawner.generatedTerrain.forewardItems)
        {
            possibleZItems.Add(item);
        }
        // put together a list of possible combinations from both sides
        foreach (var zItem in possibleZItems)
        {
            //Debug.Log("Z");
            for (int i = 0; i < possibleXItems.Count; i++)
            {
                if (possibleXItems[i].gameObject.name.Contains(zItem.gameObject.name))
                {
                    possibleChunksToSpawn.Add(possibleXItems[i]);
                    possibleSelectedTerrainIndexes.Add(i);
                }
            }
        }
    }

    void GenerateNextBigChunk()
    {
        transform.parent.GetComponent<RailSpawner>().GenerateNextChunk();
    }

    void SetFirstGeneratedTerrain(List<GameObject> slopeItems)
    {
        if (prevZSpawner != null)
        {
            possibleXItems = slopeItems;
            CollectAndComparePossibleTerrainsToSpawn();
            rand = Random.Range(0, possibleChunksToSpawn.Count);
            SelectItem();
        }
        else
        {
            rand = Random.Range(0, slopeItems.Count);
            for (int i = 0; i < slopeItems.Count; i++)
            {
                possibleSelectedTerrainIndexes.Add(i);
            }
            generatedTerrain = slopeItems[rand].GetComponent<PossibleChunks>();
        }
    }

    int GetChunkAltitudeStepMatch(string matchName)
    {
        if (spawningDirection == false)
        {
            for (int i = 0; i < generatedTerrain.leftItems.Length; i++)
            {
                if (generatedTerrain.rightItems[i].name == matchName)
                {
                    return i;
                }
            }
        }
        else
        {
            for (int i = 0; i < generatedTerrain.leftItems.Length; i++)
            {
                if (generatedTerrain.leftItems[i].name == matchName)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    ConnectedSpawner GetThisChunkSpawner(int number)
    {
        foreach (ConnectedSpawner item in thisChunkGenerator.connectedSpawners)
        {
            if (number == item.currentChunkFromTrack)
            {
                if (item.spawningDirection == spawningDirection)
                {
                    return item;
                }
            }
        }
        return null;
    }

    void SelectItem()
    {
        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(false);
        }

        if (possibleChunksToSpawn.Count == 0) // if there are still no possible combinations from both sides
        {
            if (prevXSpawner != null)
            {
                Debug.LogWarning("There is a missing terrain tile in chunk " + thisChunkGenerator.chunkIndex + " on the tile number " + currentChunkFromTrack + " in the (" + spawningDirection + ") direction. The previous Z chunk was: " + prevZSpawner.generatedTerrain.gameObject.name + ", and the previous X chunk was: " + prevXSpawner.generatedTerrain.gameObject.name + ". A new chunk type for this combination would be needed.");
            }
            else
            {
                Debug.LogWarning("There is no previous X spawner since this is a first tile from the track and also there is no possible connection to fit both X and Z. The rail slope is " + transform.parent.GetComponent<RailSpawner>().startingSlopeType + " and the previous Z spawner is " + prevZSpawner.generatedTerrain.gameObject.name);
            }

            SelectXItems();
        }

        // must check, even if there are possible chunks to spawn from both sides
        // note: this is a temporary solution for the lack of matching terrain chunks which is countered by finding the next possible chunk that aligns with the previous Z terrain
        CheckForSlopeNeed();

        foreach (var item in leftStraightItems)
        {
            if (foundItem == false) if (CheckItem(item) == true) foundItem = true;
        }
        foreach (var item in rightStraightItems)
        {
            if (foundItem == false) if (CheckItem(item) == true) foundItem = true;
        }
        foreach (var item in leftDownItems)
        {
            if (foundItem == false) if (CheckItem(item) == true) foundItem = true;
        }
        foreach (var item in leftUpItems)
        {
            if (foundItem == false) if (CheckItem(item) == true) foundItem = true;
        }
        foreach (var item in rightDownItems)
        {
            if (foundItem == false) if (CheckItem(item) == true) foundItem = true;
        }
        foreach (var item in rightUpItems)
        {
            if (foundItem == false) if (CheckItem(item) == true) foundItem = true;
        }
    }

    bool CheckItem(GameObject item)
    {
        if (possibleChunksToSpawn[rand].name == item.name)
        {
            if (item.name.Contains("12"))
            {
                //Debug.LogError("12.. and I don't know why");
            }
            item.SetActive(true);
            generatedTerrain = item.GetComponent<PossibleChunks>();
            return true;
        }
        return false;        
    }

    void CheckForSlopeNeed()
    {
        // a technically more accurate temporary solution for this would be to separate the chunk slope types into even further slope types being stronger up or down slopes (those that change height by 2 chunk height units rather than 1, aka y 12 rather than y 6 which is the normal height change). Here we are generalising both the simple 1 height unit and 2 height units as just up or down slopings

        if (prevZSpawner == null) return;

        if (prevZSpawner.transform.position.y > transform.position.y)
        {
            // force spawn a chunk going up
            Debug.LogWarning("The current chunk is getting out of hand, need to go upwards");

            if (spawningDirection == true)
            {
                foreach (Transform item in transform)
                {
                    if (item.GetComponent<PossibleChunks>().leftSloping == PossibleChunks.LeftSloping.Up)
                    {
                        if (possibleChunksToSpawn.Contains(item.gameObject))
                        {
                            possibleXSlopes.Add(item.gameObject);
                            Debug.LogWarning("left up");
                        }
                    }
                }
            }
            else
            {
                foreach (Transform item in transform)
                {
                    if (item.GetComponent<PossibleChunks>().rightSloping == PossibleChunks.RightSloping.Up)
                    {
                        if (possibleChunksToSpawn.Contains(item.gameObject))
                        {
                            possibleXSlopes.Add(item.gameObject);
                            Debug.LogWarning("right up");
                        }
                    }
                }
            }
            Debug.LogWarning("x slope count: " + possibleXSlopes.Count);
            if (possibleXSlopes.Count > 0)
            {
                possibleChunksToSpawn.Clear();
                possibleXItemsBeforeChange = possibleXItems;
                possibleXItems = possibleXSlopes;
                SelectXItems();
            }
        }
        else if (prevZSpawner.transform.position.y < transform.position.y)
        {
            // force spawn a chunk going down
            Debug.LogWarning("The current chunk is getting out of hand, need to go downwards");

            if (spawningDirection == true)
            {
                foreach (Transform item in transform)
                {
                    if (item.GetComponent<PossibleChunks>().leftSloping == PossibleChunks.LeftSloping.Down)
                    {
                        if (possibleXItems.Contains(item.gameObject))
                        {
                            possibleXSlopes.Add(item.gameObject);
                            Debug.LogWarning("left down");
                        }
                    }
                }
            }
            else
            {
                foreach (Transform item in transform)
                {
                    if (item.GetComponent<PossibleChunks>().rightSloping == PossibleChunks.RightSloping.Down)
                    {
                        if (possibleXItems.Contains(item.gameObject))
                        {
                            possibleXSlopes.Add(item.gameObject);
                            Debug.LogWarning("right down");
                        }
                    }
                }
            }
            Debug.LogWarning("x slope count: " + possibleXSlopes.Count);
            if (possibleXSlopes.Count > 0)
            {
                possibleChunksToSpawn.Clear();
                possibleXItemsBeforeChange = possibleXItems;
                possibleXItems = possibleXSlopes;
                SelectXItems();
            }
        }

        if ((Mathf.Abs(prevZSpawner.currentAltitude) - Mathf.Abs(currentAltitude)) > 1)
        {
            Debug.LogError("The chunk differences have gone out of hand");
        }
    }

    void SelectXItems()
    {
        possibleSelectedTerrainIndexes.Clear();
        for (int i = 0; i < possibleXItems.Count; i++)
        {
            possibleChunksToSpawn.Add(possibleXItems[i]);
            possibleSelectedTerrainIndexes.Add(i);
        }
        rand = Random.Range(0, possibleChunksToSpawn.Count);
        //Debug.LogError("There is a missing terrain here - need to create a new fit");
    }

    void PoolTerrains()
    {
        if (transform.childCount < 9)
        {
            foreach (GameObject item in spawningTerrains)
            {
                Instantiate(item, transform).SetActive(false);
            }
        }
        firstTimeSpawning = false;
        SetPossibleChunks();
    }

    void SetPossibleChunks()
    {
        leftStraightItems.Clear();
        rightStraightItems.Clear();
        leftDownItems.Clear();
        leftUpItems.Clear();
        rightDownItems.Clear();
        rightUpItems.Clear();

        PossibleChunks possibleChunk;
        foreach (Transform item in transform)
        {
            if (item.GetComponent<PossibleChunks>() == null)
            {
                Debug.LogError("The terrain chunk to spawn is of an incorrect type");
                return;
            }

            possibleChunk = item.GetComponent<PossibleChunks>();

            switch (possibleChunk.leftSlopeMatch)
            {
                case PossibleChunks.LeftStartingSlopeType.Straight:
                    leftStraightItems.Add(item.gameObject);
                    break;
                case PossibleChunks.LeftStartingSlopeType.Up:
                    leftUpItems.Add(item.gameObject);
                    break;
                case PossibleChunks.LeftStartingSlopeType.Down:
                    leftDownItems.Add(item.gameObject);
                    break;
                case PossibleChunks.LeftStartingSlopeType.NA:
                    break;
                default:
                    break;
            }

            switch (possibleChunk.rightSlopeMatch)
            {
                case PossibleChunks.RightStartingSlopeType.Straight:
                    rightStraightItems.Add(item.gameObject);
                    break;
                case PossibleChunks.RightStartingSlopeType.Up:
                    rightUpItems.Add(item.gameObject);
                    break;
                case PossibleChunks.RightStartingSlopeType.Down:
                    rightDownItems.Add(item.gameObject);
                    break;
                case PossibleChunks.RightStartingSlopeType.NA:
                    break;
                default:
                    break;
            }
        }
    }
}