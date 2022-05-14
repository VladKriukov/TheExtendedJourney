using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class VideoSettings : MonoBehaviour
{
    public static VideoSettings instance;

    public bool postProcessing = true;
    [SerializeField] PostProcessVolume[] postProcessVolumes;
    public bool particles = true;

    public void SetPostProcessing(bool b)
    {
        postProcessing = b;
        foreach (var item in postProcessVolumes)
        {
            item.enabled = b;
        }
    }

    public void SetParticles(bool b)
    {
        particles = b;
    }
}