using UnityEngine;
using UnityEngine.Rendering;

public class PlayerHealthVisuals : MonoBehaviour
{
    [SerializeField] VolumeProfile hitHurtEffect;
    [SerializeField] VolumeProfile lowHealth;
    [SerializeField] VolumeProfile veryLowHealth;
    [SerializeField] VolumeProfile death;

    UnityEngine.Rendering.Universal.ChromaticAberration chromaticAberration;
    UnityEngine.Rendering.Universal.FilmGrain filmGrain;
    UnityEngine.Rendering.Universal.LensDistortion lensDistortion;
    UnityEngine.Rendering.Universal.DepthOfField depthOfField;
    UnityEngine.Rendering.Universal.MotionBlur motionBlur;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TookDamage()
    {
        animator.Play("HitHurtEffect");
    }

    public void CheckHealth(float playerHealth)
    {
        animator.SetFloat("Health", playerHealth);
        if (playerHealth <= 0)
        {
            animator.SetTrigger("Death");
        }
    }

    public void SetProcessing(bool b)
    {
        if (!hitHurtEffect.TryGet(out chromaticAberration)) throw new System.NullReferenceException(nameof(chromaticAberration));
        chromaticAberration.active = b;

        if (!lowHealth.TryGet(out chromaticAberration)) throw new System.NullReferenceException(nameof(chromaticAberration));
        chromaticAberration.active = b;
        if (!lowHealth.TryGet(out filmGrain)) throw new System.NullReferenceException(nameof(filmGrain));
        filmGrain.active = b;

        if (!veryLowHealth.TryGet(out chromaticAberration)) throw new System.NullReferenceException(nameof(chromaticAberration));
        chromaticAberration.active = b;
        if (!veryLowHealth.TryGet(out filmGrain)) throw new System.NullReferenceException(nameof(filmGrain));
        filmGrain.active = b;
        if (!veryLowHealth.TryGet(out lensDistortion)) throw new System.NullReferenceException(nameof(lensDistortion));
        lensDistortion.active = b;
        if (!veryLowHealth.TryGet(out depthOfField)) throw new System.NullReferenceException(nameof(depthOfField));
        depthOfField.active = b;
        if (!veryLowHealth.TryGet(out motionBlur)) throw new System.NullReferenceException(nameof(motionBlur));
        motionBlur.active = b;

        if (!veryLowHealth.TryGet(out lensDistortion)) throw new System.NullReferenceException(nameof(lensDistortion));
        lensDistortion.active = b;
        if (!veryLowHealth.TryGet(out motionBlur)) throw new System.NullReferenceException(nameof(motionBlur));
        motionBlur.active = b;
    }
}