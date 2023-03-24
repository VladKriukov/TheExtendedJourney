using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/CraftingRecipes", order = 1)]
public class CraftingRecipes : ScriptableObject
{
    public List<Recipe> recipes = new List<Recipe>();
}

[Serializable]
public struct Recipe
{
    public string itemName;
    public Sprite itemSprite;
    public List<RecipeItem> requiredItems;
    //public List<InterchangableRecipeItems> interchangableItems;
    public GameObject output;
}

[Serializable]
public struct RecipeItem
{
    public ItemProperties.ItemType requiredItem;
    public int amount;
}

[Serializable]
public struct InterchangableRecipeItems
{
    public List<ItemProperties.ItemType> items;
    public int amount;
}