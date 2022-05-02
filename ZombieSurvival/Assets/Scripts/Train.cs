using UnityEngine;

public class Train : MonoBehaviour
{
    public float forwardForce;
    public float reverseForce;
    [SerializeField] float fuelUsage;
    [SerializeField] float dragMultiplier;
    [SerializeField] FuelTank fuelTank;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rb.drag = Mathf.Abs(rb.velocity.z * dragMultiplier);
        if (fuelTank.fuel > 0)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rb.AddForce(new Vector3(0, 0, forwardForce));
                fuelTank.fuel -= fuelUsage * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(new Vector3(0, 0, reverseForce));
                fuelTank.fuel -= fuelUsage * Time.deltaTime;
            }
        }
    }
}