using UnityEngine;

public class Train : MonoBehaviour
{
    public float forwardForce;
    public float reverseForce;
    [SerializeField] float dragMultiplier;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rb.drag = Mathf.Abs(rb.velocity.z * dragMultiplier);
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(new Vector3(0, 0, forwardForce));
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(new Vector3(0, 0, reverseForce));
        }
    }
}