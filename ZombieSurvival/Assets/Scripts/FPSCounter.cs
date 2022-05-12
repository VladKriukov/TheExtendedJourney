using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    TMP_Text counter;
    float timer = 0.25f;

    private void Awake()
    {
        counter = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (Time.unscaledTime > timer)
        {
            counter.text = "" + (int)(1 / Time.unscaledDeltaTime);
            timer = Time.unscaledTime + 1;
        }
    }
}