using System.Collections;
using UnityEngine;

public class FishingRod : MonoBehaviour
{
    private Animator animator;
    private LineRenderer rodLine;

    private bool castRod;

    [SerializeField] private GameObject bobber;
    [SerializeField] private float castingForce;
    [SerializeField] private float fishingTimerMin;
    [SerializeField] private float fishingTimerMax;
    [SerializeField] private float catchingWindow;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rodLine = bobber.transform.GetChild(0).GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && castRod == false)
        {
            castRod = true;
            animator.SetTrigger("CastRod");
            // todo: can make the rod strength stronger to throw further
        }
        else if (Input.GetMouseButtonDown(1) && castRod == true)
        {
            animator.SetTrigger("Retrieve");
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Retrieve();
        }

        rodLine.SetPosition(0, transform.GetChild(0).position);
        rodLine.SetPosition(1, bobber.transform.position);
    }

    private void Retrieve()
    {
        bobber.GetComponent<Bobber>().baiting = false;
        bobber.transform.parent = transform.GetChild(0);
        bobber.transform.position = transform.GetChild(0).position;
        bobber.GetComponent<Rigidbody>().Sleep();
        bobber.SetActive(false);
        StopAllCoroutines();
        castRod = false;
    }

    private void CheckCatch()
    {
        if (animator.GetBool("Caught") == true)
        {
            Debug.Log("Caught something!!");
            animator.SetBool("Caught", false);
        }
    }

    private void CastBobber()
    {
        bobber.SetActive(true);
        bobber.transform.parent = null;
        bobber.GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward * castingForce);
    }

    public void HitWater()
    {
        StartCoroutine(nameof(BobberInWater));
    }

    private IEnumerator BobberInWater()
    {
        yield return new WaitForSeconds(Random.Range(fishingTimerMin, fishingTimerMax));
        animator.SetBool("Caught", true);
        yield return new WaitForSeconds(catchingWindow);
        animator.SetBool("Caught", false);
        StartCoroutine(nameof(BobberInWater));
    }
}