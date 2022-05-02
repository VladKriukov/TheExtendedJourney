using UnityEngine;

public class Seat : MonoBehaviour
{
    [SerializeField] Train controlledTrain;
    [SerializeField] Vector3 offset;
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
        controlledTrain.enabled = true;
        cooldown = 0.5f;
    }

    private void Update()
    {
        cooldown -= Time.deltaTime;
        if (player != null && Input.GetKeyDown(KeyCode.E) && cooldown <= 0)
        {
            Exit();
        }
    }

    void Exit()
    {
        player.transform.parent = null;
        player.GetComponent<FirstPersonController>().enableHeadBob = true;
        player.GetComponent<Rigidbody>().isKinematic = false;
        player.GetComponent<Collider>().enabled = true;
        player = null;
        controlledTrain.enabled = false;
    }
}