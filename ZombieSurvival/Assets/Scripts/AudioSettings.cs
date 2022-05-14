using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] AudioMixer masterMixer;

    public void ChangeMasterVolume(float volume)
    {
        var result = 20 * Mathf.Log10(volume);
        masterMixer.SetFloat("MasterVolume", result);
    }

    public void ChangeMusicVolume(float volume)
    {
        var result = 20 * Mathf.Log10(volume);
        masterMixer.SetFloat("MusicVolume", result);
    }

    public void ChangeTrainVolume(float volume)
    {
        var result = 20 * Mathf.Log10(volume);
        masterMixer.SetFloat("TrainVolume", result);
    }

    public void ChangeToolsVolume(float volume)
    {
        var result = 20 * Mathf.Log10(volume);
        masterMixer.SetFloat("ToolsVolume", result);
    }

    public void ChangeNatureVolume(float volume)
    {
        var result = 20 * Mathf.Log10(volume);
        masterMixer.SetFloat("NatureVolume", result);
    }
}