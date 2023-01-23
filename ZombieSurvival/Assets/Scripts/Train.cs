using UnityEngine;
using TMPro;

public class Train : MonoBehaviour
{
    [SerializeField] float forwardForce;
    [SerializeField] float reverseForce;
    [SerializeField] float fuelUsage;
    [SerializeField] float idleFuelUsage;
    [SerializeField] float dragMultiplier;
    [SerializeField] FuelTank fuelTank;
    [SerializeField] GameObject trainEngine;
    [SerializeField] float throttleSpeed;
    [SerializeField] TMP_Text throttleText;
    float currentThrottle;
    bool movingForward;
    AudioSource engineOff;
    float currentForce;
    AudioSource engineAudio;
    GameObject breaksAudio;

    public bool acceptingInput;
    Rigidbody rb;
    bool trainOn;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        engineOff = trainEngine.transform.GetChild(0).GetComponent<AudioSource>();
        engineAudio = trainEngine.GetComponent<AudioSource>();
        breaksAudio = trainEngine.transform.GetChild(1).gameObject;
    }

    public void StartEngine()
    {
        trainEngine.SetActive(true);
        trainOn = true;
    }

    public void StopEngine()
    {
        currentThrottle = 0;
        engineOff.Play();
        trainOn = false;
        Invoke(nameof(EngineOff), engineOff.clip.length);
    }

    void EngineOff()
    {
        trainEngine.SetActive(false);
        trainOn = false;
    }

    private void Update()
    {
        throttleText.text = "Throttle: " + (Mathf.Round(currentThrottle * 100f) / 100f);
        if (trainOn == true)
        {
            engineAudio.enabled = !Game.gamePaused;
        }
        if (fuelTank.fuel > 0 && acceptingInput == true && trainEngine.activeInHierarchy == true)
        {
            if (Input.GetKey(KeyCode.W))
            {
                if (rb.velocity.z > -0.1f)
                {
                    movingForward = true;
                    currentThrottle = Mathf.Clamp(currentThrottle + throttleSpeed * Time.deltaTime, 0, 1);
                    currentForce = forwardForce;
                    breaksAudio.SetActive(false);
                }
                else
                {
                    movingForward = false;
                    if (currentThrottle > 0 && currentForce == reverseForce)
                    {
                        currentThrottle = Mathf.Clamp(currentThrottle - throttleSpeed * Time.deltaTime, 0, 1);
                    }
                    else
                    {
                        currentForce = forwardForce;
                        breaksAudio.SetActive(true);
                    }
                }
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (rb.velocity.z < 0.1f)
                {
                    movingForward = false;
                    currentThrottle = Mathf.Clamp(currentThrottle + throttleSpeed * Time.deltaTime, 0, 1);
                    currentForce = reverseForce;
                    breaksAudio.SetActive(false);
                }
                else
                {
                    movingForward = true;
                    if (currentThrottle > 0 && currentForce == forwardForce)
                    {
                        currentThrottle = Mathf.Clamp(currentThrottle - throttleSpeed * Time.deltaTime, 0, 1);
                    }
                    else
                    {
                        currentForce = reverseForce;
                        breaksAudio.SetActive(true);
                    }
                }
            }
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
            {
                breaksAudio.SetActive(false);
                if (currentThrottle == 0) currentForce = 0;
            }
        }

        engineAudio.pitch = (currentThrottle * 2) + 1;

        if (fuelTank.fuel > 0 && trainEngine.activeInHierarchy == true)
        {
            rb.AddForce(new Vector3(0, 0, currentForce * rb.mass * Time.deltaTime * Mathf.Abs(currentThrottle)));
            fuelTank.fuel -= fuelUsage * Time.deltaTime * currentThrottle;
            fuelTank.fuel -= idleFuelUsage * Time.deltaTime;
            rb.drag = Mathf.Abs(rb.velocity.z * dragMultiplier);
        }

        if (acceptingInput)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (trainEngine.activeInHierarchy == true)
                {
                    StopEngine();
                }
                else
                {
                    StartEngine();
                }
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                engineAudio.transform.GetChild(2).GetComponent<AudioSource>().Play();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerTrigger") && Vector3.Distance(transform.position, other.transform.position) < 100)
        {
            rb.isKinematic = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerTrigger") && Vector3.Distance(transform.position, other.transform.position) > 100)
        {
            rb.isKinematic = true;
        }
    }
}