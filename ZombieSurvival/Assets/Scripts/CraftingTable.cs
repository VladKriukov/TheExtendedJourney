using System.Collections.Generic;
using UnityEngine;

public class CraftingTable : MonoBehaviour
{
    [SerializeField] CraftingBook craftingBook;
    List<GameObject> craftingTableInventory = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        craftingTableInventory.Add(other.gameObject);
        craftingBook.AddItemToCurrentItems(other.gameObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Removing item");
        craftingTableInventory.Remove(other.gameObject);
        craftingBook.RemoveItemFromCurrentItems(other.gameObject.name);
    }
}