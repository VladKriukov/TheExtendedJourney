using UnityEngine;

public class BiomeChanger : MonoBehaviour
{
    [SerializeField] int materialToChangeID;
    [SerializeField] BiomeMaterials biomeMaterials;
    
    MeshRenderer meshRenderer;
    
    Material[] cachedMaterial;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        cachedMaterial = meshRenderer.materials;
    }

    public void ChangeBiome()
    {
        switch (ChunkSpawner.currentBiome)
        {
            case ChunkSpawner.CurrentBiome.Grass:
                cachedMaterial[materialToChangeID] = biomeMaterials.grass;
                break;
            case ChunkSpawner.CurrentBiome.Snow:
                cachedMaterial[materialToChangeID] = biomeMaterials.snow;
                break;
            case ChunkSpawner.CurrentBiome.Sand:
                cachedMaterial[materialToChangeID] = biomeMaterials.sand;
                break;
            case ChunkSpawner.CurrentBiome.Fall:
                cachedMaterial[materialToChangeID] = biomeMaterials.fall;
                break;
            default:
                break;
        }
        meshRenderer.materials = cachedMaterial;
    }
}