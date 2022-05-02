using UnityEngine;
using TMPro;

public class CameraInterractor : MonoBehaviour
{
    public float rayDistance;
    public LayerMask layerMask;
    //[SerializeField] private GameObject placingObject;
    [SerializeField] private Transform holdingLocation;
    [SerializeField] private Transform toolLocation;
    [SerializeField] private float holdingLerpSpeed = 50;
    [SerializeField] Animator itemNameDisplay;
    private bool hitting;
    public bool holdingItem;
    public bool holdingTool;
    private GameObject pickUpObject;
    Player player;

    private void Awake()
    {
        player = transform.parent.parent.GetComponent<Player>();        
    }

    private void Update()
    {
        //if (PauseManager.paused == true) return;

        RaycastHit hit;

        hitting = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, rayDistance, layerMask);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

        if (holdingItem == true)
        {
            //LerpToHoldingLocation();
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (pickUpObject.GetComponent<Tool>() != null) pickUpObject.GetComponent<Tool>().heldInHands = false;
                if (pickUpObject.GetComponent<Animator>() != null) pickUpObject.GetComponent<Animator>().enabled = false;
                pickUpObject.GetComponent<Rigidbody>().useGravity = true;
                pickUpObject.GetComponent<Rigidbody>().isKinematic = false;
                pickUpObject.GetComponent<Collider>().enabled = true;
                pickUpObject.layer = 0;
                pickUpObject.transform.parent = null;
                pickUpObject.transform.position = holdingLocation.position;
                pickUpObject = null;
                holdingItem = false;
                holdingTool = false;
                itemNameDisplay.ResetTrigger("ShowItemName");
                return;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (pickUpObject.GetComponent<ItemProperties>() != null && pickUpObject.GetComponent<ItemProperties>().consumable)
                {
                    player.AddFood(pickUpObject.GetComponent<ItemProperties>().foodValue);
                    Destroy(pickUpObject);
                    pickUpObject = null;
                    holdingItem = false;
                }
            }
            return;
        }

        if (hitting == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hit.collider.CompareTag("PickUpAble"))
                {
                    pickUpObject = hit.collider.gameObject;
                    if (pickUpObject.transform.parent != null)
                    {
                        if (pickUpObject.transform.parent.GetComponent<PhysicsStopper>() != null)
                        {
                            pickUpObject.transform.parent.GetComponent<PhysicsStopper>().RemoveItem(pickUpObject);
                        }
                    }
                    if (pickUpObject.GetComponent<Tool>() != null)
                    {
                        ClosePickup();
                        pickUpObject.GetComponent<Tool>().heldInHands = true;
                        holdingTool = true;
                    }
                    else if (pickUpObject.GetComponent<ItemProperties>() != null && pickUpObject.GetComponent<ItemProperties>().consumable)
                    {
                        ClosePickup();
                    }
                    else
                    {
                        FarPickup();
                    }
                    pickUpObject.GetComponent<Rigidbody>().useGravity = false;
                    pickUpObject.GetComponent<Rigidbody>().isKinematic = true;
                    pickUpObject.GetComponent<Collider>().enabled = false;
                    pickUpObject.layer = 9;
                    holdingItem = true;
                    itemNameDisplay.GetComponent<TMP_Text>().text = pickUpObject.name;
                    itemNameDisplay.SetTrigger("ShowItemName");
                }
                if (hit.collider.GetComponent<Seat>() != null)
                {
                    hit.collider.GetComponent<Seat>().Sit(transform.parent.parent.gameObject);
                }
            }
            if (Input.GetMouseButtonDown(0)) // left click
            {
                if (hit.collider.GetComponent<Interactable>() != null)
                {
                    hit.collider.GetComponent<Interactable>().Interract();
                }
                if (hit.collider.gameObject.name == "trigger")
                {
                    hit.transform.parent.GetComponent<Animator>().SetTrigger("Play");
                }
            }
            /*
            if (Input.GetMouseButtonDown(1)) // right click
            {
                if (hit.collider.GetComponent<Interactable>() != null)
                {
                    hit.collider.GetComponent<Interactable>().SecondaryInterraction();
                }
            }
            if (Input.GetMouseButton(1)) // holding right click
            {
                if (hit.collider.GetComponent<Interactable>() == null)
                {
                    if (selector.CheckAmount(selector.currentSelection) == true)
                    {
                        placingObject.SetActive(true);
                        placingObject.transform.position = hit.point;
                    }
                    else
                    {
                        placingObject.SetActive(false);
                    }
                }
                else
                {
                    placingObject.SetActive(false);
                }
            }
            */
        }
        else
        {
            //placingObject.SetActive(false);
        }
    }

    void ClosePickup()
    {
        pickUpObject.transform.parent = toolLocation;
        if (pickUpObject.GetComponent<ItemProperties>() != null)
        {
            pickUpObject.transform.localPosition = pickUpObject.GetComponent<ItemProperties>().pickupLocationOffset;
        }
        else
        {
            pickUpObject.transform.position = toolLocation.position;
            pickUpObject.transform.rotation = toolLocation.rotation;
        }
        
    }

    void FarPickup()
    {
        pickUpObject.transform.parent = holdingLocation;
        if (pickUpObject.GetComponent<ItemProperties>() != null)
        {
            
            pickUpObject.transform.localPosition = pickUpObject.GetComponent<ItemProperties>().pickupLocationOffset;
        }
        else
        {
            pickUpObject.transform.position = holdingLocation.position;
            pickUpObject.transform.rotation = holdingLocation.rotation;
        }
    }

    void LerpToHoldingLocation()
    {
        pickUpObject.GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(pickUpObject.transform.position, holdingLocation.position, Time.deltaTime * holdingLerpSpeed));
        pickUpObject.GetComponent<Rigidbody>().MoveRotation(Quaternion.Lerp(pickUpObject.transform.rotation, holdingLocation.rotation, Time.deltaTime * holdingLerpSpeed));
    }
}