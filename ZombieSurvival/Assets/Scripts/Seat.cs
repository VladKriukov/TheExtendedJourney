using UnityEngine;

public class Seat : MonoBehaviour
{
    [SerializeField] Train controlledTrain;
    [SerializeField] Vector3 offset;
    [SerializeField] GameObject trainUI;
    GameObject player;
    float cooldown = 0.5f;

    public void Sit(GameObject _player)
    {
        player = _player;
        player.transform.parent = transform;
        player.transform.position = transform.position + offset;
        player.GetComponent<FirstPersonController>().enableHeadBob = false;
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.GetComponent<Collider>().enabled = false;
        controlledTrain.acceptingInput = true;
        controlledTrain.StartEngine();
        cooldown = 0.5f;
        trainUI.SetActive(true);
    }

    private void Update()
    {
        cooldown -= Time.deltaTime;
        if (player != null && Input.GetKeyDown(KeyCode.E) && cooldown <= 0)
        {
            Exit();
        }
        if (controlledTrain.acceptingInput == true && Input.GetKeyDown(KeyCode.F1))
        {
            trainUI.transform.GetChild(0).gameObject.SetActive(!trainUI.transform.GetChild(0).gameObject.activeInHierarchy);
        }
    }

    void Exit()
    {
        player.transform.parent = null;
        player.GetComponent<FirstPersonController>().enableHeadBob = true;
        player.GetComponent<Rigidbody>().isKinematic = false;
        player.GetComponent<Collider>().enabled = true;
        player = null;
        controlledTrain.acceptingInput = false;
        trainUI.SetActive(false);
    }
}