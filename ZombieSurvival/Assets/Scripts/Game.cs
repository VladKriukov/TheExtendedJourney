using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Game : MonoBehaviour
{
    // this could be done to be set in game menu settings
    public static int minAltitudeSteps = -7;
    public static int maxAltitudeSteps = 7;
    public static int maxChunksFromTrack = 5;

    public static float totalProgressParts;
    public static float progress;
    [SerializeField] Image progressBar;
    [SerializeField] TMP_Text progressNumber;
    [SerializeField] GameObject menuCamera;
    [SerializeField] GameObject player;
    [SerializeField] Slider altitudeStepsSlider;
    [SerializeField] Slider chunksFromTrackSlider;

    bool loading;

    private void Awake()
    {
        altitudeStepsSlider.value = maxAltitudeSteps;
        chunksFromTrackSlider.value = maxChunksFromTrack;
    }

    public void SetAltitudeSteps(float value)
    {
        minAltitudeSteps = -(int)value;
        maxAltitudeSteps = (int)value;
    }

    public void SetMaxChunksFromTrack(float value)
    {
        maxChunksFromTrack = (int)value;
    }

    public void StartLoading()
    {
        loading = true;
    }

    private void Update()
    {
        if (loading == true)
        {
            progressBar.fillAmount = progress / totalProgressParts;
            progressNumber.text = "" + Mathf.CeilToInt(progress / totalProgressParts * 100) + "%";
            if (progress >= totalProgressParts)
            {
                GetComponent<Animator>().SetTrigger("Play");
                menuCamera.SetActive(false);
                player.SetActive(true);
                loading = false;
            }
        }
    }
}