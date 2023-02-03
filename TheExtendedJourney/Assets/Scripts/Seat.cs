using UnityEngine;

public class Seat : MonoBehaviour
{
    [SerializeField] Train controlledTrain;
    [SerializeField] Vector3 offset;
    [SerializeField] GameObject trainUI;
    GameObject player;
    float cooldown = 0.5f;
    bool helpOn = true;

    public void Sit(GameObject _player)
    {
        player = _player;
        player.transform.parent = transform;
        player.transform.position = transform.position + offset;
        player.GetComponent<FirstPersonController>().enableHeadBob = false;
        player.GetComponent<FirstPersonController>().enableSprint = false;
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
        if (controlledTrain.acceptingInput == true && helpOn == true)
        {
            trainUI.gameObject.SetActive(!Game.gamePaused);
        }
        if (player != null && Input.GetKeyDown(KeyCode.E) && cooldown <= 0)
        {
            Exit();
        }
        if (controlledTrain.acceptingInput == true && Input.GetKeyDown(KeyCode.F1) && Game.gamePaused == false)
        {
            trainUI.gameObject.SetActive(!trainUI.transform.GetChild(0).gameObject.activeInHierarchy);
            helpOn = !helpOn;
        }
    }

    void Exit()
    {
        player.transform.parent = null;
        player.GetComponent<FirstPersonController>().enableHeadBob = true;
        player.GetComponent<FirstPersonController>().enableSprint = true;
        player.GetComponent<Rigidbody>().isKinematic = false;
        player.GetComponent<Collider>().enabled = true;
        player = null;
        controlledTrain.acceptingInput = false;
        trainUI.SetActive(false);
    }
}