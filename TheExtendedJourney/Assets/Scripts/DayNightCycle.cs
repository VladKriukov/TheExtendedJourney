using UnityEngine;
using UnityEngine.Audio;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [Range(-80, 0)] public float dayTimeVolume;
    [Range(-80, 0)] public float nightTimeVolume;

    private void Update()
    {
        audioMixer.SetFloat("DayVolume", dayTimeVolume);
        audioMixer.SetFloat("NightVolume", nightTimeVolume);
    }

    public void Day()
    {

    }

    public void Night()
    {

    }
}