using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SetSliderTextValue : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    private void OnEnable() // on enable so that awake could set the slider value settings from save
    {
        text.text = "" + GetComponent<Slider>().value;
    }

    public void SetTextValue(float value)
    {
        text.text = "" + value;
    }
}