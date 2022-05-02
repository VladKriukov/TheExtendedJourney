using UnityEngine;

public class ItemProperties : MonoBehaviour
{
    public string itemDisplayName;

    public bool fuelConsumable;
    public float fuelValue;

    public bool consumable;
    public float foodValue;

    public Vector3 pickupLocationOffset;
    public Quaternion picupRotationOffset;

    private void Awake()
    {
        name = itemDisplayName;
    }
}