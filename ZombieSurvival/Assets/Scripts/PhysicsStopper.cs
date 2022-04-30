using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhysicsStopper : MonoBehaviour
{
    [SerializeField] float lockWaitTime;
    List<GameObject> waitingItems = new List<GameObject>();
    List<GameObject> itemsStored = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") != true)
        {
            waitingItems.Add(other.gameObject);
            StartCoroutine(LockItem(other.gameObject));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("On trigger exit");
        if (other.CompareTag("Player") != true)
        {
            waitingItems.Remove(other.gameObject);
            //RemoveItem(other.gameObject);
        }
    }

    IEnumerator LockItem(GameObject item)
    {
        yield return new WaitForSeconds(lockWaitTime);
        if (waitingItems.Contains(item))
        {
            while (item.GetComponent<Rigidbody>().velocity.magnitude > 0.25f)
            {
                yield return new WaitForSeconds(0.5f);
            }
            if (item.transform.parent == null)
            {
                waitingItems.Remove(item);
                item.GetComponent<Rigidbody>().isKinematic = true;
                item.transform.parent = transform;
                itemsStored.Add(item);
            }
        }
        yield return null;
    }

    public void RemoveItem(GameObject itemToRemove)
    {
        if (itemsStored.Contains(itemToRemove))
        {
            itemsStored.Remove(itemToRemove);
            itemToRemove.GetComponent<Rigidbody>().isKinematic = false;
            itemToRemove.transform.parent = null;
        }
    }
}