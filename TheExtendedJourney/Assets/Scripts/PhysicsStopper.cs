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
        if (other.CompareTag("PickUpAble") == true)
        {
            waitingItems.Add(other.gameObject);
            StartCoroutine(LockItem(other.gameObject));
        }
    }

    private void OnTriggerExit(Collider other)
    {
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
            while (item != null)
            { 
                if (item.GetComponent<Rigidbody>().velocity.magnitude > 0.25f)
                {
                    yield return new WaitForSeconds(0.5f);
                    
                }
                else
                {
                    break;
                }
            }

            if (item != null)
            {
                waitingItems.Remove(item);
                if (item.transform.parent == null)
                {
                    Destroy(item.GetComponent<Rigidbody>());
                    //item.GetComponent<Rigidbody>().drag = 9999;
                    //item.GetComponent<Rigidbody>().angularDrag = 9999;
                    item.transform.parent = transform;
                    itemsStored.Add(item);
                }
            }
        }
        yield return null;
    }

    public void RemoveItem(GameObject itemToRemove)
    {
        if (itemsStored.Contains(itemToRemove))
        {
            itemToRemove.AddComponent<Rigidbody>();
            //itemToRemove.GetComponent<Rigidbody>().drag = 0;
            //itemToRemove.GetComponent<Rigidbody>().angularDrag = 0;
            itemToRemove.transform.parent = null;
            itemsStored.Remove(itemToRemove);
        }
    }
}