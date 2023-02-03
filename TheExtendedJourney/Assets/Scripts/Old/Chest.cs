using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour
{
    [SerializeField] Transform lootSpawnLocation;
    public GameObject[] chestItems;
    Animation anim;
    bool canOpenChest = false;
    bool chestOpened = false;

    private void Start()
    {
        anim = GetComponent<Animation>();
    }

    private void Update()
    {
        if (canOpenChest && Input.GetKeyDown(KeyCode.E) && chestOpened == false)
        {
            StartCoroutine(WaitForChest());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canOpenChest = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canOpenChest = false;
        }
    }

    IEnumerator WaitForChest()
    {
        chestOpened = true;
        anim.Play();
        yield return new WaitForSeconds(anim["ChestAnim"].length);
        int randSelection = Random.Range(0, chestItems.Length);
        Instantiate(chestItems[randSelection], lootSpawnLocation.transform.position, lootSpawnLocation.transform.rotation);
    }
}