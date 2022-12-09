using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : MonoBehaviour
{
    [SerializeField] CraftingBook craftingBook;
    List<GameObject> craftingTableInventory = new List<GameObject>();

    List<bool> requiredItemAvailable = new List<bool>();
    List<bool> interchangableItemAvailable = new List<bool>();

    int recipeID = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ItemProperties>().craftingItem == true)
        {
            if (craftingTableInventory.Contains(other.gameObject) == false)
            {
                craftingTableInventory.Add(other.gameObject);
                craftingBook.AddItemToCurrentItems(other.gameObject.name);
                Craft("Pickaxe");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        RemoveItem(other.gameObject);
    }

    public void RemoveItem(GameObject item)
    {
        if (item.GetComponent<ItemProperties>().craftingItem == true)
        {
            //Debug.Log("Removing item");
            craftingTableInventory.Remove(item);
            craftingBook.RemoveItemFromCurrentItems(item.name);
        }
    }

    public void Craft(string itemToCraft)
    {
        for (int i = 0; i < craftingBook.craftingRecipes.recipes.Count; i++)
        {
            if (itemToCraft == craftingBook.craftingRecipes.recipes[i].itemName)
            {
                recipeID = i;
            }
        }
        CheckForItemAvailability();
        foreach (var item in requiredItemAvailable)
        {
            if (item == false)
            {
                return;
            }
        }
        Instantiate(craftingBook.craftingRecipes.recipes[recipeID].output, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.identity);
        ConsumeResources();
    }

    private void CheckForItemAvailability()
    {
        requiredItemAvailable.Clear();

        foreach (var item in craftingBook.craftingRecipes.recipes[recipeID].requiredItems)
        {
            List<GameObject> availableSpecifiedItem = new List<GameObject>();
            foreach (var currentItem in craftingTableInventory)
            {
                if (currentItem.GetComponent<ItemProperties>().itemType == item.requiredItem)
                {
                    Debug.Log("Required item type matched");
                    availableSpecifiedItem.Add(currentItem);
                }
            }
            if (availableSpecifiedItem.Count >= item.amount)
            {
                requiredItemAvailable.Add(true);
            }
            else
            {
                requiredItemAvailable.Add(false);
            }
        }
    }

    private void ConsumeResources()
    {
        foreach (var item in craftingBook.craftingRecipes.recipes[recipeID].requiredItems)
        {
            for (int i = 0; i < item.amount; i++)
            {
                foreach (var currentItem in craftingTableInventory)
                {
                    if (currentItem.GetComponent<ItemProperties>() == null) break;
                    if (item.requiredItem == currentItem.GetComponent<ItemProperties>().itemType)
                    {
                        craftingTableInventory.Remove(currentItem);
                        Destroy(currentItem);
                        break;
                    }
                }
            }
        }
    }

    private void CheckForInterchangeableCraftingAvailability(int recipeID)
    {
        interchangableItemAvailable.Clear();
        /*
        foreach (var item in craftingBook.craftingRecipes.recipe[recipeID].interchangableItems.items)
        {

        }
        */
    }
}