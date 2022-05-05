using UnityEngine;

public class ItemProperties : MonoBehaviour
{
    public string itemDisplayName;

    [Header("Fuel")]
    public bool fuelConsumable;
    public float fuelValue;

    [Header("Food")]
    public bool consumable;
    public float foodValue;

    [Header("Pickup")]
    public bool closePickup;
    public Vector3 pickupLocationOffset;
    public Quaternion picupRotationOffset;

    private void Awake()
    {
        name = itemDisplayName;
    }
}