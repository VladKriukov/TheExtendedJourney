using UnityEngine;

public class Tool : MonoBehaviour
{
    [System.Flags] public enum EffectiveAgainst
    {
        NA = 1 << 0,
        Rocks = 1 << 1,
        Trees = 1 << 2,
        Enemies = 1 << 3
    }
    public EffectiveAgainst effectiveAgainst;
    public float effectiveDamage;
    public float ineffectiveDamage;
    [SerializeField] float damageCooldown;
    [SerializeField] float durability = -1; // -1 for infinite
    [HideInInspector] public bool heldInHands;
    Animator animator;
    RaycastHit hit;

    private bool hitting;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.enabled = heldInHands;
        if (heldInHands == false) return;

        hitting = Physics.Raycast(transform.parent.parent.position, transform.parent.parent.TransformDirection(Vector3.forward), out hit, transform.parent.parent.GetComponent<CameraInterractor>().rayDistance, transform.parent.parent.GetComponent<CameraInterractor>().layerMask);

        //if (hitting == true) Debug.Log(hit.collider.gameObject.name);

        if (Input.GetMouseButton(0) && heldInHands)
        {
            animator.SetTrigger("UseTool");
        }
        if (Input.GetMouseButtonUp(0))
        {
            animator.ResetTrigger("UseTool");
        }
    }

    // called from the animator
    void SendDamage()
    {
        if (hitting == true)
        {
            if (hit.collider.gameObject.GetComponent<Resource>() != true) return;

            switch (hit.collider.gameObject.GetComponent<Resource>().effectiveItem)
            {
                case Resource.EffectiveItem.NA:
                    hit.collider.GetComponent<Resource>().Damage(ineffectiveDamage, hit.point, hit.normal);
                    break;
                case Resource.EffectiveItem.Rocks:
                    if (effectiveAgainst.HasFlag(EffectiveAgainst.Rocks))
                    {
                        hit.collider.gameObject.GetComponent<Resource>().Damage(effectiveDamage, hit.point, hit.normal);
                    }
                    else
                    {
                        hit.collider.gameObject.GetComponent<Resource>().Damage(ineffectiveDamage, hit.point, hit.normal);
                    }
                    break;
                case Resource.EffectiveItem.Trees:
                    if (effectiveAgainst.HasFlag(EffectiveAgainst.Trees))
                    {
                        hit.collider.gameObject.GetComponent<Resource>().Damage(effectiveDamage, hit.point, hit.normal);
                    }
                    else
                    {
                        hit.collider.gameObject.GetComponent<Resource>().Damage(ineffectiveDamage, hit.point, hit.normal);
                    }
                    break;
                case Resource.EffectiveItem.Enemies:
                    if (effectiveAgainst.HasFlag(EffectiveAgainst.Enemies))
                    {
                        hit.collider.gameObject.GetComponent<Resource>().Damage(effectiveDamage, hit.point, hit.normal);
                    }
                    else
                    {
                        hit.collider.gameObject.GetComponent<Resource>().Damage(ineffectiveDamage, hit.point, hit.normal);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
