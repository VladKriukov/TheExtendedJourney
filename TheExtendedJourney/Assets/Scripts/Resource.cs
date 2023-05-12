using UnityEngine;
using System.Collections.Generic;

public class Resource : MonoBehaviour
{
    public enum EffectiveItem
    {
        NA,
        Rocks,
        Trees,
        Enemies
    }
    public EffectiveItem effectiveItem;
    [SerializeField] bool damagable;
    [SerializeField] float health;
    [SerializeField] List<GameObject> drops = new List<GameObject>();
    [SerializeField] int numberOfDrops;
    [SerializeField] Vector3 spawnRange;
    [SerializeField] Vector3 spawnOffset;
    [SerializeField] Quaternion rotation;
    [SerializeField] GameObject healthPopupGO;
    AudioSource audioSource;
    HealthPopup healthPopup;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Damage(float amount, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
        
        if (damagable == false) return;
        health -= amount;
        if (healthPopup == null)
        {
            healthPopup = Instantiate(healthPopupGO, transform).GetComponent<HealthPopup>();
        }
        healthPopup.transform.position = hitPoint;
        healthPopup.transform.LookAt(FindObjectOfType<Player>().transform.position);
        healthPopup.ShowHealth(health + amount, health);
        if(gameObject.GetComponent<AnimalAI>() != null)
        {
            if (health <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void DropLoot()
    {
        for (int i = 0; i < numberOfDrops; i++)
        {
            Instantiate(drops[Random.Range(0, drops.Count)], transform.position + new Vector3(Random.Range(-spawnRange.x, spawnRange.x) + spawnOffset.x, Random.Range(-spawnRange.y, spawnRange.y) + spawnOffset.y, Random.Range(-spawnRange.z, spawnRange.z) + spawnOffset.z), rotation);
        }
        //healthPopup.transform.parent = null;
        gameObject.SetActive(false);
    }
}
