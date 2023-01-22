using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingTable : MonoBehaviour
{
    [SerializeField] CraftingBook craftingBook;
    [SerializeField] Transform craftingContentsMenu;
    [SerializeField] GameObject craftingItemPanel;
    List<GameObject> craftingTableInventory = new List<GameObject>();

    List<bool> requiredItemAvailable = new List<bool>();
    List<bool> interchangableItemAvailable = new List<bool>();

    List<GameObject> craftingItems = new List<GameObject>();

    int recipeID = 0;

    private void Awake()
    {
        for (int i = 0; i < craftingBook.craftingRecipes.recipes.Count; i++)
        {
            craftingItems.Add(Instantiate(craftingItemPanel, craftingContentsMenu));
            craftingItems[i].transform.name = craftingBook.craftingRecipes.recipes[i].itemName;
            if (craftingBook.craftingRecipes.recipes[i].itemSprite != null)
            {
                craftingItems[i].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = craftingBook.craftingRecipes.recipes[i].itemSprite;
                craftingItems[i].transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < craftingItems.Count; i++)
        {
            craftingItems[i].transform.GetChild(0).GetComponent<Button>().interactable = CheckForItemAvailability(i);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ItemProperties>().craftingItem == true)
        {
            if (craftingTableInventory.Contains(other.gameObject) == false)
            {
                craftingTableInventory.Add(other.gameObject);
                craftingBook.AddItemToCurrentItems(other.gameObject.name);
                
                //Craft("Pickaxe");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        RemoveItem(other.gameObject);
        for (int i = 0; i < craftingItems.Count; i++)
        {
            craftingItems[i].transform.GetChild(0).GetComponent<Button>().interactable = CheckForItemAvailability(i);
        }
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

    public void Craft(GameObject item)
    {
        Craft(item.name);
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

        if (craftingBook.craftingRecipes.recipes[recipeID].output == null)
        {
            return;
        }

        if (CheckForItemAvailability() == false) return;

        Instantiate(craftingBook.craftingRecipes.recipes[recipeID].output, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.identity);
        ConsumeResources();
    }

    bool CheckForItemAvailability()
    {
        return CheckForItemAvailability(recipeID);
    }

    private bool CheckForItemAvailability(int id)
    {
        requiredItemAvailable.Clear();
        foreach (var item in craftingBook.craftingRecipes.recipes[id].requiredItems)
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
        return itemAvailability();
    }

    // this could be expanded on to retrieve item availability for each item, rather than the full thing
    bool itemAvailability()
    {
        if (requiredItemAvailable.Count > 0)
        {
            foreach (var item in requiredItemAvailable)
            {
                if (item == false)
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            return false;
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