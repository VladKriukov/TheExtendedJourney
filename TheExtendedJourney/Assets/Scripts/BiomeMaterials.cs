using UnityEngine;

[CreateAssetMenu(fileName = "BiomeMaterials", menuName = "ScriptableObjects/BiomeMaterials", order = 1)]
public class BiomeMaterials : ScriptableObject
{
    public Material dirt;
    public Material grass;
    public Material snow;
    public Material sand;
    public Material fall;
}