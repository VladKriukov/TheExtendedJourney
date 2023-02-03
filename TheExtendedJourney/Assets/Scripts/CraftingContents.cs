using System.Collections.Generic;
using UnityEngine;

public class CraftingContents : MonoBehaviour
{
    List<Transform> items = new List<Transform>();
    int indexFirst = 0;
    int indexLast = 2;

    private void Start()
    {
        foreach (Transform item in transform)
        {
            items.Add(item);
            item.gameObject.SetActive(false);
        }
        for (int i = 0; i < items.Count; i++)
        {
            if (i > indexLast) break;
            items[i].gameObject.SetActive(true);
        }
    }

    public void Right()
    {
        if (indexLast < transform.childCount - 1)
        {
            indexFirst++;
            indexLast++;
            ShowItems();
        }
        else
        {
            // maybe try cyclical contents
        }
    }

    public void Left()
    {
        if (indexFirst > 0)
        {
            indexFirst--;
            indexLast--;
            ShowItems();
        }
        else
        {
            // maybe try cyclical contents
        }
    }

    void ShowItems()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].gameObject.SetActive(false);
            if (i >= indexFirst && i <= indexLast)
            {
                items[i].gameObject.SetActive(true);
            }
        }
    }
}