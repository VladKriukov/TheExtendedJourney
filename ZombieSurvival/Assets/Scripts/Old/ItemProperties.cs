using UnityEngine;
using UnityEngine.UI;

public class ItemProperties : MonoBehaviour
{
    public string itemDisplayName;

    [Header("Fuel")]
    public bool fuelConsumable;
    public float fuelValue;

    [Header("Food")]
    public bool consumable;
    public float foodValue;
    public float healthValue;

    [Header("Pickup")]
    public bool closePickup;
    public Vector3 pickupLocationOffset;
    public Quaternion picupRotationOffset;
    public float holdingRayOffset;

    [Header("SpawningOffset")]
    public Vector3 spawningOffset;

    [Header("Crafting")]
    public bool craftingItem;
    public enum ItemType
    {
        NA, Stick, Log, Stone
    }
    public ItemType itemType;
    public Image itemIcon;

    private void Awake()
    {
        name = itemDisplayName;
    }
}