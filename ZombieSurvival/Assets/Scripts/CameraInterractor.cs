using UnityEngine;

public class CameraInterractor : MonoBehaviour
{
    public float rayDistance;
    public LayerMask layerMask;
    //[SerializeField] private GameObject placingObject;
    [SerializeField] private Transform holdingLocation;
    [SerializeField] private Transform toolLocation;
    [SerializeField] private float holdingLerpSpeed = 50;

    private bool hitting;
    public bool holdingItem;
    public bool holdingTool;
    private GameObject pickUpObject;
    

    private void Update()
    {
        //if (PauseManager.paused == true) return;

        RaycastHit hit;

        hitting = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, rayDistance, layerMask);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

        if (holdingItem == true)
        {
            //LerpToHoldingLocation();
            if (Input.GetKeyDown(KeyCode.E))
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
                return;
            }
        }

        if (holdingItem == true) return;

        if (hitting == true)
        {
            if (hit.collider.CompareTag("PickUpAble"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    pickUpObject = hit.collider.gameObject;
                    if (pickUpObject.GetComponent<Tool>() != null)
                    {
                        pickUpObject.transform.parent = toolLocation;
                        pickUpObject.transform.position = toolLocation.position;
                        pickUpObject.transform.rotation = toolLocation.rotation;
                        pickUpObject.GetComponent<Collider>().enabled = false;
                        pickUpObject.GetComponent<Tool>().heldInHands = true;
                        holdingTool = true;
                    }
                    else
                    {
                        pickUpObject.transform.parent = holdingLocation;
                        pickUpObject.transform.position = holdingLocation.position;
                        pickUpObject.transform.rotation = holdingLocation.rotation;
                    }
                    pickUpObject.GetComponent<Rigidbody>().useGravity = false;
                    pickUpObject.GetComponent<Rigidbody>().isKinematic = true;
                    pickUpObject.layer = 9;
                    holdingItem = true;
                    
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

    void LerpToHoldingLocation()
    {
        pickUpObject.GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(pickUpObject.transform.position, holdingLocation.position, Time.deltaTime * holdingLerpSpeed));
        pickUpObject.GetComponent<Rigidbody>().MoveRotation(Quaternion.Lerp(pickUpObject.transform.rotation, holdingLocation.rotation, Time.deltaTime * holdingLerpSpeed));
    }
}