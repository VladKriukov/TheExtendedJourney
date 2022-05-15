using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] float minStartingScale;
    [SerializeField] float maxStartingScale;
    [SerializeField] float targetScale;
    [SerializeField] float growthSpeed;
    Rigidbody rb;
    float scale;
    bool growing = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        scale = Random.Range(minStartingScale, maxStartingScale);
        transform.localScale = new Vector3(scale, scale, scale);
    }

    private void Update()
    {
        if (growing == true)
        {
            if (transform.localScale.x < targetScale)
            {
                scale += growthSpeed * Time.deltaTime;
                transform.localScale = new Vector3(scale, scale, scale);
            }
            else
            {
                rb.isKinematic = false;
                growing = false;
            }
        }
    }
}