using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour {

    public static bool gameIsPaused;

    public AudioSource audioSource;
    public GameObject pauseMenuUI;
    public Slider latencySlider;

    void Awake()
    {
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (gameIsPaused)
            Options.instance.latency = latencySlider.value;
    }

    public void PauseButtonHandler()
    {
        if (gameIsPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        gameIsPaused = true;
        Time.timeScale = 0;
        audioSource.Pause();
        pauseMenuUI.SetActive(true);
    }

    public void Resume()
    {
        gameIsPaused = false;
        Time.timeScale = 1;
        audioSource.Play();
        pauseMenuUI.SetActive(false);
    }
}
