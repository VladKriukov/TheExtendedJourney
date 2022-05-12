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
    public float healthValue;

    [Header("Pickup")]
    public bool closePickup;
    public Vector3 pickupLocationOffset;
    public Quaternion picupRotationOffset;
    public float holdingRayOffset;

    private void Awake()
    {
        name = itemDisplayName;
    }
}