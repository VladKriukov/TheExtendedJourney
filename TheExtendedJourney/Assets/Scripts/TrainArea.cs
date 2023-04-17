using UnityEngine;

public class TrainArea : MonoBehaviour
{
    FirstPersonController player;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //other.transform.parent = null;
            player.trainZVelocity = 0;
            player = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //other.transform.parent = transform.parent;
            player = other.GetComponent<FirstPersonController>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.trainZVelocity = transform.parent.GetComponent<Rigidbody>().velocity.z;
        }
    }
}