using UnityEngine;

public class Bobber : MonoBehaviour
{
    [SerializeField] FishingRod fishingRod;

    [HideInInspector] public bool baiting;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water") && baiting == false)
        {
            Debug.Log("Hit Water");
            fishingRod.HitWater();
            baiting = true;
        }
    }
}