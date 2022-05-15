using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] Camera playerCam;
    [SerializeField] float pickupRange = 10f;

    PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            FireRaycast();
        }
    }

    private void FireRaycast()
    {
        RaycastHit objectHit;

        if(Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out objectHit, pickupRange))
        {
            switch (objectHit.transform.tag)
            {
                case "Apple":
                    playerHealth.IncreasePlayerHunger(objectHit.transform.GetComponent<Food>().foodStrength);
                    Destroy(objectHit.transform.gameObject);
                    break;
                default:
                    break;
            }
        }
        else
        {
            return;
        }
    }
}