using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Game : MonoBehaviour
{
    // this could be done to be set in game menu settings
    public static int minAltitudeSteps = -7;
    public static int maxAltitudeSteps = 7;
    public static int maxChunksFromTrack = 5;

    public static int chunkPropMultiplier = 1;
    public static int chunkPropDensity = 1;

    public static float totalProgressParts;
    public static float progress;
    [SerializeField] Image progressBar;
    [SerializeField] TMP_Text progressNumber;
    [SerializeField] GameObject menuCamera;
    [SerializeField] GameObject player;
    [SerializeField] Slider altitudeStepsSlider;
    [SerializeField] Slider chunksFromTrackSlider;

    Animator animator;

    bool loading;

    bool gamePaused;
    bool inGame;

    private void Awake()
    {
        altitudeStepsSlider.value = maxAltitudeSteps;
        chunksFromTrackSlider.value = maxChunksFromTrack;
        animator = GetComponent<Animator>();
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

    public void SetChunkPropMultiplier(float value)
    {
        chunkPropMultiplier = (int)value;
    }

    public void SetChunkDensityMultiplier(float value)
    {
        chunkPropDensity = (int)value;
    }

    public void StartLoading()
    {
        loading = true;
    }

    public void PauseMenu()
    { 
        // the pause menu isn't working as the mouse gets re-captured by the fps controller
        /*
        if (!inGame) return;

        gamePaused = !gamePaused;
        animator.SetBool("Pause", gamePaused);

        if (gamePaused == true)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        player.GetComponent<FirstPersonController>().enabled = !gamePaused;
        */
    }

    private void Update()
    {
        if (loading == true)
        {
            progressBar.fillAmount = progress / totalProgressParts;
            progressNumber.text = "" + Mathf.Clamp(Mathf.CeilToInt(progress / totalProgressParts * 100), 0, 100) + "%";
            if (progress >= totalProgressParts)
            {
                animator.SetTrigger("Play");
                menuCamera.SetActive(false);
                player.SetActive(true);
                loading = false;
                inGame = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu();
        }
    }
}