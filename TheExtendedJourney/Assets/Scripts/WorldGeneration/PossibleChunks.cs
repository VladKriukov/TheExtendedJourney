using UnityEngine;

public class PossibleChunks : MonoBehaviour
{
    // the possible terrains from this chunk that the next spawner can spawn
    public GameObject[] leftItems;
    public GameObject[] rightItems;
    public GameObject[] forewardItems;
    public GameObject[] backwardItems;
    // based on the type of terrain given above, this might be necessary to be a -1 or a +1 if it is a slope
    // this is to make sure the next terrain spawns at the right altitude
    public int[] requiredLeftAltitudeSteps;
    public int[] requiredRightAltitudeSteps;
    public int[] requiredForewardAltitudeSteps;
    public int[] requiredBackwardAltitudeSteps;

    public int leftStartingStep;
    public int rightStartingStep;
    // the slope starting types to connect to the rails

    [SerializeField] int topMaterialElementID;
    [SerializeField] Material dirt;
    [SerializeField] Material grass;
    [SerializeField] Material snow;
    [SerializeField] Material sand;
    [SerializeField] Material fall;

    MeshRenderer meshRenderer;
    Material[] cachedMaterial;
    Material[] newMaterial;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        cachedMaterial = meshRenderer.materials;
    }

    public void ChangeChunkBiome()
    {
        newMaterial = new Material[cachedMaterial.Length];
        for (int i = 0; i < cachedMaterial.Length; i++)
        {
            newMaterial[i] = dirt;
        }
        switch (ChunkSpawner.currentBiome)
        {
            case ChunkSpawner.CurrentBiome.Grass:
                newMaterial[topMaterialElementID] = grass;
                break;
            case ChunkSpawner.CurrentBiome.Snow:
                newMaterial[topMaterialElementID] = snow;
                break;
            case ChunkSpawner.CurrentBiome.Sand:
                newMaterial[topMaterialElementID] = sand;
                break;
            case ChunkSpawner.CurrentBiome.Fall:
                newMaterial[topMaterialElementID] = fall;
                break;
            default:
                break;
        }
        meshRenderer.materials = newMaterial;
    }

    public enum LeftStartingSlopeType
    {
        Straight, Up, Down, NA
    }
    public LeftStartingSlopeType leftSlopeMatch;
    public enum RightStartingSlopeType
    {
        Straight, Up, Down, NA
    }
    public RightStartingSlopeType rightSlopeMatch;

    public enum LeftSloping
    {
        Straight, Up, Down
    }
    public LeftSloping leftSloping;

    public enum RightSloping
    {
        Straight, Up, Down
    }
    public RightSloping rightSloping;
}