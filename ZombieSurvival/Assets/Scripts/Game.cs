using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    // this could be done to be set in game menu settings
    public static int minAltitudeSteps = -7;
    public static int maxAltitudeSteps = 7;
    public static int maxChunksFromTrack = 5;
    public static int railFlatness = 5;
    public static int numberOfChunksToSpawn = 10;
    public static int waterLevel = 2;

    public static int chunkPropMultiplier = 1;
    public static float chunkPropDensity = 1;

    public static float totalProgressParts;
    public static float progress;

    public static Stats stats;
    [SerializeField] Image progressBar;
    [SerializeField] TMP_Text progressNumber;
    [SerializeField] GameObject menuCamera;
    [SerializeField] GameObject player;
    [SerializeField] Slider altitudeStepsSlider;
    [SerializeField] Slider chunksFromTrackSlider;
    [SerializeField] List<TMP_Text> distanceTravelledTexts = new List<TMP_Text>();

    Animator animator;

    bool loading;

    public static bool gamePaused;
    public static bool inGame;

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

    public void SetRailFlatness(float value)
    {
        railFlatness = (int)value;
    }

    public void SetTerrainDistance(float value)
    {
        numberOfChunksToSpawn = (int)value;
    }

    public void SetChunkPropMultiplier(float value)
    {
        chunkPropMultiplier = (int)value;
    }

    public void SetChunkDensityMultiplier(float value)
    {
        chunkPropDensity = value;
    }

    public void SetWaterLevel(float value)
    {
        waterLevel = (int)value - 1;
    }

    public void SetFogDensity(float value)
    {
        RenderSettings.fogDensity = value;
    }

    public void StartLoading()
    {
        loading = true;
    }

    public void PauseMenu()
    {
        if (!inGame) return;

        gamePaused = !gamePaused;
        animator.SetBool("Paused", gamePaused);

        if (gamePaused == true)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        player.SetActive(!gamePaused);
        menuCamera.SetActive(gamePaused);
    }

    public void PlayAgain()
    {
        progress = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
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
        if (Input.GetMouseButtonDown(0) && gamePaused == true)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        foreach (var item in distanceTravelledTexts)
        {
            item.text = "" + (Mathf.Round(stats.furthestDistanceTravelled * 10f) / 10f) + "m";
        }
    }
}

public struct Stats
{
    public float furthestDistanceTravelled;
    // possibly add more to show in stats
}