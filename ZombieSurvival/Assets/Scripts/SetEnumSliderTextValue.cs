using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SetEnumSliderTextValue : SetSliderTextValue
{
    [SerializeField] VideoSettings videoSettings;
    public List<int> targetFrameRates = new List<int>();
    Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = targetFrameRates.Count - 1;
        slider.value = slider.maxValue; // todo: change to load from settings
        SetTargetFrameRate(targetFrameRates.Count - 1);
    }

    public void SetTargetFrameRate(float value)
    {
        SetTextValue(targetFrameRates[(int)value]);
        videoSettings.SetTargetFrameRate(targetFrameRates[(int)value]);
    }
}