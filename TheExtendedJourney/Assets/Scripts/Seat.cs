using UnityEngine;

public class Seat : MonoBehaviour
{
    [SerializeField] Train controlledTrain;
    [SerializeField] Vector3 offset;
    [SerializeField] GameObject trainUI;
    GameObject player;
    float cooldown = 0.5f;
    bool helpOn = true;
    float playerUnderstanding;

    public void Sit(GameObject _player)
    {
        player = _player;
        controlledTrain.player = player.GetComponent<FirstPersonController>();
        controlledTrain.playerStartingFOV = player.GetComponent<FirstPersonController>().fov;
        player.transform.parent = transform;
        player.transform.position = transform.position + offset;
        player.GetComponent<FirstPersonController>().enableHeadBob = false;
        player.GetComponent<FirstPersonController>().enableSprint = false;
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.GetComponent<Collider>().enabled = false;
        controlledTrain.acceptingInput = true;
        controlledTrain.StartEngine();
        cooldown = 0.5f;
        if (playerUnderstanding < 1)
        {
            trainUI.SetActive(true);
        }
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

        if (playerUnderstanding < 1 && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.H) || Input.GetKey(KeyCode.F)))
        {
            playerUnderstanding += Time.deltaTime * 0.1f;
            if (playerUnderstanding >= 1)
            {
                trainUI.SetActive(false);
                helpOn = !helpOn;
            }
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
        controlledTrain.player = null;
        controlledTrain.acceptingInput = false;
        player.GetComponent<FirstPersonController>().fov = controlledTrain.playerStartingFOV;
        player = null;
        trainUI.SetActive(false);
    }
}