using UnityEngine;
using UnityEngine.Rendering;
using TMPro;
using System.Collections.Generic;

public class VideoSettings : MonoBehaviour
{
    public static VideoSettings instance;

    public bool postProcessing = true;
    [SerializeField] Volume[] volumes;
    public bool particles = true;

    [SerializeField] TMP_Dropdown resolutionDropdown;
    List<string> resolutionOptions = new List<string>();
    Resolution[] resolutions;

    private void Awake()
    {
        // todo: after load settings
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionOptions.Add(resolutions[resolutions.Length - i - 1].width + "x" + resolutions[resolutions.Length - i - 1].height);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = resolutions.Length - i - 1;
                //Debug.Log(currentResolutionIndex);
            }
        }
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutions.Length - resolutionIndex - 1];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullScreen(bool b)
    {
        Screen.fullScreen = b;
    }

    public void SetTargetFrameRate(int rate)
    {
        Application.targetFrameRate = rate;
    }

    public void SetPostProcessing(bool b)
    {
        postProcessing = b;
        foreach (var item in volumes)
        {
            item.enabled = b;
        }
    }

    public void SetParticles(bool b)
    {
        particles = b;
    }
}