using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthPopup : MonoBehaviour
{
    [SerializeField] float displayTime;
    [SerializeField] float lerpTime;
    Image health;
    Animator animator;
    float targetHealth;
    bool noHP;

    private void Awake()
    {
        health = transform.GetChild(1).GetComponent<Image>();
        animator = GetComponent<Animator>();
        animator.SetTrigger("Show");
    }

    private void Update()
    {
        if (noHP == true) return;

        health.fillAmount -= (health.fillAmount - targetHealth) * Time.deltaTime * lerpTime;
        if (health.fillAmount <= 0)
        {
            transform.parent.GetComponent<Resource>().DropLoot();
            noHP = true;
        }
    }

    public void ShowHealth(float _currentHealth, float _targetHealth)
    {
        StopAllCoroutines();
        animator.SetTrigger("Show");
        animator.ResetTrigger("Hide");
        targetHealth = _targetHealth / _currentHealth;
        StartCoroutine(nameof(HidingHealth));
        //Invoke(nameof(HideHealth), displayTime);
        //todo: make manual invoke timer
    }

    IEnumerator HidingHealth()
    {
        yield return new WaitForSeconds(displayTime);
        HideHealth();
    }

    void HideHealth()
    {
        animator.SetTrigger("Hide");
        animator.ResetTrigger("Show");
    }
}