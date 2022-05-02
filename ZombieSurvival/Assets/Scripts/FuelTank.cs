using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FuelTank : MonoBehaviour
{
    public float fuel;
    public float maxFuelCapacity;
    public float startingFuelAmount;
    [SerializeField] Image guage;
    [SerializeField] Gradient gradient;

    ItemProperties item;

    private void Awake()
    {
        fuel = startingFuelAmount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ItemProperties>() != null)
        {
            item = other.GetComponent<ItemProperties>();
            if (item.fuelConsumable == false) return;

            StartCoroutine(nameof(ConsumeFuel));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ItemProperties>() != null)
        {
            StopAllCoroutines();
        }
    }

    IEnumerator ConsumeFuel()
    {
        while (item.transform.parent != null)
        {
            yield return new WaitForEndOfFrame();
        }

        fuel = Mathf.Clamp(fuel + item.fuelValue, 0, maxFuelCapacity);
        if (item.transform.parent != null)
        {
            Debug.Log("Parent: " + item.transform.parent.name);
        }
        Destroy(item.gameObject);
        item = null;
        yield return null;
    }

    private void Update()
    {
        guage.fillAmount = fuel / maxFuelCapacity;
        guage.color = gradient.Evaluate(1 / (maxFuelCapacity / fuel));
    }
}