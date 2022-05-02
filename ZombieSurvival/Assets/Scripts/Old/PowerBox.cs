using UnityEngine;

public class PowerBox : MonoBehaviour
{
    public GameObject[] lights;
    bool canInterract;
    bool on;

    private void Update()
    {
        if (canInterract)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SwitchLights();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInterract = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canInterract = false;
        }
    }

    void SwitchLights()
    {
        on = !on;
        foreach (var item in lights)
        {
            if (item.GetComponent<Light>() != null)
                item.GetComponent<Light>().enabled = on;
        }
    }
}