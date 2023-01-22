using UnityEngine;

public class CraftingButton : MonoBehaviour
{
    public void Craft()
    {
        transform.parent.parent.parent.parent.GetComponent<CraftingTable>().Craft(name);
    }
}