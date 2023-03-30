using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTable : MonoBehaviour
{
    [SerializeField] CraftingBook upgradeBook;
    public List<GameObject> upgradeInventory = new List<GameObject>();

    List<bool> requiredItemAvailable = new List<bool>();
    Train train;
    FuelTank fTank;
    int recipeID = 0;
    float fuelToAdd;
    void Start()
    {
        train = FindObjectOfType<Train>();
        fTank = FindObjectOfType<FuelTank>();
        fuelToAdd = fTank.maxFuelCapacity * 0.5f;
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ItemProperties>().craftingItem == true)
        {
            if(upgradeInventory.Contains(other.gameObject) == false)
            {
                upgradeInventory.Add(other.gameObject);
                upgradeBook.AddItemToCurrentItems(name);
            }
        }
        if(CheckForItemAvailability(0))
        {
            Upgrade(0);
            
        }
    }
    private bool CheckForItemAvailability(int id)
    {
        requiredItemAvailable.Clear();
        foreach (var item in upgradeBook.craftingRecipes.upgrade[id].itemNeeded)
        {
            List<GameObject> availableSpecifiedItem = new List<GameObject>();
            foreach (var currentItem in upgradeInventory)
            {
                if (currentItem.GetComponent<ItemProperties>().itemType == item.requiredItem)
                {
                    //Debug.Log("Required item type matched");
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
    private void OnTriggerExit(Collider other)
    {
        RemoveItem(other.gameObject);
    }
    public void RemoveItem(GameObject item)
    {
        if (item.GetComponent<ItemProperties>().craftingItem == true)
        {
            GetComponent<PhysicsStopper>().RemoveItem(item);
            Debug.Log("Removing item");
            upgradeInventory.Remove(item);
            upgradeBook.RemoveItemFromCurrentItems(item.name);
        }
    }
    public void Upgrade(int upgradeToDo)
    {
        switch (upgradeToDo)
        {
            case 0:
                train.fuelUsage = (train.fuelUsage * 0.9f);
                train.idleFuelUsage = (train.idleFuelUsage * 0.9f);
                ConsumeResources();
                break;
            case 1:
                train.dragMultiplier = (train.dragMultiplier * 0.9f);
                break;
            case 2:
                fTank.maxFuelCapacity += fuelToAdd;
                break;
            default:
                break;
        }
    }
    private void ConsumeResources()
    {
        foreach (var item in upgradeBook.craftingRecipes.upgrade[recipeID].itemNeeded)
        {
            for (int i = 0; i < item.amount; i++)
            {
                foreach (var currentItem in upgradeInventory)
                {
                    if (currentItem.GetComponent<ItemProperties>() == null) break;
                    if (item.requiredItem == currentItem.GetComponent<ItemProperties>().itemType)
                    {
                        upgradeInventory.Remove(currentItem);
                        Destroy(currentItem);
                        break;
                    }
                }
            }
        }
    }
}
