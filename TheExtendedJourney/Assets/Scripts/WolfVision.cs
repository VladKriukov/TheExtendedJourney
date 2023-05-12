using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfVision : MonoBehaviour
{
    AnimalAI animal;
    void Start()
    {
        animal = GetComponentInParent<AnimalAI>();
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player") { animal.chaseTarget = other.gameObject; }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player") {animal.chaseTarget = null; }
    }
}
