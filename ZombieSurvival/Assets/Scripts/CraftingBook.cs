using UnityEngine;
using TMPro;

public class CraftingBook : MonoBehaviour, Interactable
{
    [SerializeField] Animator mainMenu;
    [SerializeField] GameObject currentItemPrefab;
    [SerializeField] Transform itemViewport;
    GameObject item;

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