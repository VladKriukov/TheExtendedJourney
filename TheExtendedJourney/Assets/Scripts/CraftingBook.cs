using TMPro;
using UnityEngine;

public class CraftingBook : MonoBehaviour, Interactable
{
    [SerializeField] private Animator mainMenu;
    [SerializeField] private GameObject currentItemPrefab;
    [SerializeField] private Transform itemViewport;
    public CraftingRecipes craftingRecipes;
    private GameObject item;

    private void Awake()
    {
        for (int i = 0; i < craftingRecipes.recipes.Count; i++)
        {
            //Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            //Debug.Log("Recipe: " + craftingRecipes.recipes[i].itemName);
            //Debug.Log("Items required: ");
            foreach (var item in craftingRecipes.recipes[i].requiredItems)
            {
                //Debug.Log(item.requiredItem.name + ", " + item.amount + "x");
            }
            /*
            Debug.Log("Interchangable items:");
            foreach (var interchangeableItem in craftingRecipes.recipes[i].interchangableItems)
            {
                Debug.Log("------");
                foreach (var item in interchangeableItem.items)
                {
                    //Debug.Log(item.name);
                }
                Debug.Log("Amount: " + interchangeableItem.amount + "x");
            }
            */
        }
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E)) && mainMenu.GetCurrentAnimatorStateInfo(0).IsName("CraftingBook"))
        {
            mainMenu.SetTrigger("Back");
        }
    }

    public void AddItemToCurrentItems(string itemName)
    {
        item = Instantiate(currentItemPrefab, itemViewport);
        item.transform.GetChild(0).GetComponent<TMP_Text>().text = itemName;
    }

    public void RemoveItemFromCurrentItems(string itemName)
    {
        for (int i = 0; i < itemViewport.childCount; i++)
        {
            if (itemViewport.GetChild(i).GetChild(0).name == itemName)
            {
                Destroy(item.gameObject);
            }
        }
    }

    public void Interract()
    {
        mainMenu.SetTrigger("CraftingBook");
    }
}