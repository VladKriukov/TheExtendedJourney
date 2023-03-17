using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemVariants", menuName = "ScriptableObjects/ItemVariants", order = 1)]
public class ItemVariants : ScriptableObject
{
    public List<GameObject> itemVariants_Normal = new List<GameObject>();
    public List<GameObject> itemVariants_Snow = new List<GameObject>();
    public List<GameObject> itemVariants_Sand = new List<GameObject>();
    public List<GameObject> itemVariants_Fall = new List<GameObject>();
}