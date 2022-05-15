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
        if (value.ToString().Length > 3)
        {
            text.text = value.ToString().Substring(0, 3);
        }
        else
        {
            text.text = value.ToString();
        }
    }
}