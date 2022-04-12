using UnityEngine;

public class SpawnableItem : MonoBehaviour
{
    bool spawned;

    private void Start()
    {
        Invoke(nameof(SetKinematic), 0.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (spawned == false && !collision.gameObject.name.Contains("Grass"))
        {
            gameObject.SetActive(false);
        }
    }

    void SetKinematic()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }
}