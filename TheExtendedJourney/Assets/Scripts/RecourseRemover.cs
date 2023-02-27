using UnityEngine;

public class RecourseRemover : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Resource>() != null || other.GetComponent<ItemProperties>() != null)
        {
            other.gameObject.SetActive(false);
        }
    }
}
