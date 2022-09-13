using UnityEngine;
using UnityEngine.UI;

public class HealthPopup : MonoBehaviour
{
    [SerializeField] float displayTime;
    [SerializeField] float lerpTime;
    Image health;
    Animator animator;
    float targetHealth;

    private void Awake()
    {
        health = transform.GetChild(1).GetComponent<Image>();
        animator = GetComponent<Animator>();
        animator.SetTrigger("Show");
    }

    private void Update()
    {
        health.fillAmount -= (health.fillAmount - targetHealth) * Time.deltaTime * lerpTime;
    }

    public void ShowHealth(float _currentHealth, float _targetHealth)
    {
        StopAllCoroutines();
        animator.SetTrigger("Show");
        animator.ResetTrigger("Hide");
        targetHealth = _targetHealth / _currentHealth;
        Invoke(nameof(HideHealth), displayTime);
    }

    void HideHealth()
    {
        animator.SetTrigger("Hide");
        animator.ResetTrigger("Show");
    }
}