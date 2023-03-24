using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/CraftingRecipes", order = 1)]
public class CraftingRecipes : ScriptableObject
{
    public List<Recipe> recipes = new List<Recipe>();
    public List<UpgradeRecipie> upgrade = new List<UpgradeRecipie>();//This is the list of upgrade recipies
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

[Serializable]//This and the below RequiredItems gives the recipie for the upgrade, the output is used in a switch statement for the upgrade script
public struct UpgradeRecipie
{
    public string upgradeName;
    public List<RecipeItem> itemNeeded;
    public int output;
}

[Serializable]
public struct RequiredItem
{
    public ItemProperties.ItemType type;
    public int ammount;
}