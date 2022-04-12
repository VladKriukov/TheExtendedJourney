using UnityEngine;
using System.Collections.Generic;

public class ConnectedSpawner : MonoBehaviour
{
    [SerializeField] GameObject connectedSpawner;
    [SerializeField] float connectedSpawnerOffset;

    public List<GameObject> spawningTerrains = new List<GameObject>(); // GameObjects to instantiate

    public PossibleChunks generatedTerrain; // the terrain that was selected to generate for this spawner

    public ConnectedSpawner prevXSpawner;
    public ConnectedSpawner prevZSpawner;

    public int currentAltitude;
    public int currentChunkFromTrack;
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

        //Debug.Log("Connected -------------------- Spawner");
        //Debug.Log("Current Altitude: " + currentAltitude);
        //Debug.Log("Current Chunk From Track: " + currentChunkFromTrack);

        //transform.position = new Vector3(transform.position.x, transform.position.y + currentAltitude * chunkSpawner.chunkYOffset, transform.position.z);

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

            //Debug.Log("This is the second (or further) chunk");
            ConnectedSpawner backwardsChunk = chunkSpawner.GetChunk(thisChunkGenerator.chunkIndex - 1).connectedSpawners[i];
            if (backwardsChunk.currentChunkFromTrack == _currentChunkFromTrack && backwardsChunk.spawningDirection == spawningDirection)
            {
                prevZSpawner = backwardsChunk;
                break;
            }
            
        }
        if (prevZSpawner == null && thisChunkGenerator.chunkIndex != 0)
        {
            //Debug.LogError("Strange - the previous spawner was not found");
        }

        //Debug.Log("Spawning direction" + spawningDirection);
        //Debug.Log(slopeType);
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
                //Debug.Log("Previous terrain: " + prevXSpawner.generatedTerrain.name);

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
                /*
                foreach (var item in possibleXItems)
                {
                    Debug.LogWarning("PossibleXItems: " + item.name);
                }

                /*
                // collect all the possible combinations in the horizontal direction
                if (spawningDirection == false)
                {
                    foreach (var item in prevXSpawner.generatedTerrain.leftItems)
                    {
                        possibleXItems.Add(item);
                    }
                }
                else
                {
                    foreach (var item in prevXSpawner.generatedTerrain.rightItems)
                    {
                        possibleXItems.Add(item);
                    }
                }*/

                if (prevZSpawner != null)
                {
                    /*if (spawningDirection == true)
                    {
                        switch (prevXSpawner.generatedTerrain.leftSlopeMatch)
                        {
                            case PossibleChunks.LeftStartingSlopeType.Straight:
                                possibleXItems = leftStraightItems;
                                break;
                            case PossibleChunks.LeftStartingSlopeType.Up:
                                possibleXItems = leftUpItems;
                                break;
                            case PossibleChunks.LeftStartingSlopeType.Down:
                                possibleXItems = leftDownItems;
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
                                possibleXItems = rightStraightItems;
                                break;
                            case PossibleChunks.RightStartingSlopeType.Up:
                                possibleXItems = rightUpItems;
                                break;
                            case PossibleChunks.RightStartingSlopeType.Down:
                                possibleXItems = rightDownItems;
                                break;
                            case PossibleChunks.RightStartingSlopeType.NA:
                                break;
                            default:
                                break;
                        }
                    }*/

                    //Debug.Log("There is a previous Z spawner with item: " + prevZSpawner.generatedTerrain.name);
                    CollectAndComparePossibleTerrainsToSpawn();
                }
                else
                {
                    //Debug.Log("There was no previous Z spawner");
                    for (int i = 0; i < possibleXItems.Count - 1; i++)
                    {
                        possibleChunksToSpawn.Add(possibleXItems[i]);
                        possibleSelectedTerrainIndexes.Add(i);
                        //Debug.Log("Possible terrains: " + possibleChunksToSpawn[i].name);
                    }
                }
                
                rand = Random.Range(0, possibleChunksToSpawn.Count);

                SelectItem();

                // if current chunk needs to change the height in the direction of the previous chunk for the previous chunk, then change height
                if (spawningDirection == false)
                {
                    // get previous chunk and find it in the list of own possible chunks from that direction
                    //if (prevXSpawner.generatedTerrain.rightItems.Length != 5) break;

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
                    //if (prevXSpawner.generatedTerrain.leftItems.Length != 5) break;

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

        //Debug.Log("Random number: " + rand);
        //Debug.Log("Generated terrain: " + generatedTerrain);
        //Debug.Log("Number of indexes: " + possibleSelectedTerrainIndexes.Count);
        //Debug.Log("Rand: " + rand);
        //Debug.Log("Possible chunks to spawn: " + possibleChunksToSpawn.Count);
        if (currentChunkFromTrack < Game.maxChunksFromTrack)
        {
            if (spawningDirection == false)
            {
                Debug.LogWarning("Left altitude step count: " + generatedTerrain.requiredLeftAltitudeSteps.Length + ", possible selected terrain indexes count: " + possibleSelectedTerrainIndexes.Count + ", possible chunks count: " + possibleChunksToSpawn.Count + ", rand: " + rand);
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
                Debug.LogWarning("Right altitude step count: " + generatedTerrain.requiredRightAltitudeSteps.Length + ", possible selected terrain indexes count: " + possibleSelectedTerrainIndexes.Count + ", possible chunks count: " + possibleChunksToSpawn.Count + ", rand: " + rand + ", possible terrain index number: " + possibleSelectedTerrainIndexes[rand]);
                if (nextSpawnerExists == false)
                {
                    thisChunkGenerator.connectedSpawners.Add(Instantiate(connectedSpawner, new Vector3(transform.position.x + chunkSpawner.chunkXOffset, transform.position.y, transform.position.z), Quaternion.identity, transform.parent).GetComponent<ConnectedSpawner>());
                    generatedConnectedSpawner = thisChunkGenerator.connectedSpawners[thisChunkGenerator.connectedSpawners.Count - 1];
                }

                if (generatedConnectedSpawner != null)
                {
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
            //Debug.LogWarning("Added a previous Z item: " + item.name);
            possibleZItems.Add(item);
        }
        //Debug.Log("Possible X Items count: " + possibleXItems.Count);
        //Debug.Log("Possible Z Items count: " + possibleZItems.Count);
        // put together a list of possible combinations from both sides
        foreach (var zItem in possibleZItems)
        {
            //Debug.Log("Z");
            for (int i = 0; i < possibleXItems.Count; i++)
            {
                //Debug.Log("Looping through and finding a match between: " + zItem.name + " and " + possibleXItems[i].name);
                if (possibleXItems[i].gameObject.name.Contains(zItem.gameObject.name))
                {
                    possibleChunksToSpawn.Add(possibleXItems[i]);
                    possibleSelectedTerrainIndexes.Add(i);
                    //Debug.Log("Possible final terrains: " + possibleChunksToSpawn[possibleChunksToSpawn.Count - 1].name);
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
                //Debug.Log("Possible terrains: " + slopeItems[i].name);
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
                    //Debug.Log("Found match on the right side");
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
                    //Debug.Log("Found match on the left side");
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

            if (prevZSpawner.currentAltitude > currentAltitude)
            {
                // force spawn a chunk going up
                Debug.LogWarning("The current chunk is getting out of hand, need to go upwards");
            }
            else if (prevZSpawner.currentAltitude < currentAltitude)
            {
                // force spawn a chunk going down
                Debug.LogWarning("The current chunk is getting out of hand, need to go downwards");
            }

            if ((Mathf.Abs(prevZSpawner.currentAltitude) - Mathf.Abs(currentAltitude)) > 1)
            {
                Debug.LogError("The chunk differences have gone out of hand");
            }

            //todo: possibly refactor as this is repeated twice
            for (int i = 0; i < possibleXItems.Count; i++)
            {
                possibleChunksToSpawn.Add(possibleXItems[i]);
                possibleSelectedTerrainIndexes.Add(i);
                //Debug.Log("Possible terrains: " + possibleChunksToSpawn[i].name);
            }
            rand = Random.Range(0, possibleChunksToSpawn.Count);
        }

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
        //Debug.LogWarning("Possible chunk count: " + possibleChunksToSpawn.Count);
        //Debug.LogWarning("Possible X itmes count: " + possibleXItems.Count);
        if (possibleChunksToSpawn[rand].name == item.name)
        {
            //Debug.Log("Selected Item");
            item.SetActive(true);
            generatedTerrain = item.GetComponent<PossibleChunks>();
            return true;
        }
        return false;        
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

            if (spawningDirection == false)
            {
                
            }
            else
            {
                
            }
        }
        //Debug.Log("Straight left items: " + leftStraightItems.Count);
        //Debug.Log("Up left items: " + leftUpItems.Count);
        //Debug.Log("Down left items: " + leftDownItems.Count);
        //Debug.Log("Straight right items: " + rightStraightItems.Count);
        //Debug.Log("Up right items: " + rightUpItems.Count);
        //Debug.Log("Down right items: " + rightDownItems.Count);
    }
}