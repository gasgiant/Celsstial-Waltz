using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameState gameState = GameState.NotStarted;

    public static GameManager instance;

    public AudioSource audioSource;
    public GameObject pauseMenuUI;
    public GameObject startMenuUI;
    public GameObject pauseButton;
    public GameObject joystick;
    public Slider latencySlider;

    void Awake()
    {
        instance = this;
        pauseMenuUI.SetActive(false);
        pauseButton.SetActive(false);
        joystick.SetActive(false);
        startMenuUI.SetActive(true);
        latencySlider.value = Options.instance.latency;
    }

    void Update()
    {
        if (gameState == GameState.Paused)
            Options.instance.latency = latencySlider.value;

    }

    public void StartGame()
    {
        gameState = GameState.Normal;
        Metronome.instance.Restart();
        pauseButton.SetActive(true);
        joystick.SetActive(true);
        startMenuUI.SetActive(false);

        audioSource.time = 0;
        audioSource.pitch = 1;
        audioSource.Play();
    }

    public void PauseButtonHandler()
    {
        if (gameState == GameState.Paused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        gameState = GameState.Paused;
        Time.timeScale = 0;
        audioSource.Pause();
        pauseMenuUI.SetActive(true);
        joystick.SetActive(false);
    }

    public void Resume()
    {
        gameState = GameState.Normal;
        Time.timeScale = 1;
        audioSource.Play();
        pauseMenuUI.SetActive(false);
        joystick.SetActive(true);
    }

    public void GameOver()
    {
        gameState = GameState.NotStarted;
        StartCoroutine(GameOverRoutine(1.3f));
    }

    IEnumerator GameOverRoutine(float duration, float pitch = 0.2f)
    {
        pauseMenuUI.SetActive(false);
        pauseButton.SetActive(false);
        joystick.SetActive(false);
        

        float t = 0;
        while (t < 1)
        {
            audioSource.pitch = Mathf.Lerp(1, pitch, t);
            t += Time.deltaTime / duration;
            yield return new WaitForEndOfFrame();
        }
        audioSource.Pause();

        PlayerController.instance.Reset();
        startMenuUI.SetActive(true);
    }

    public enum GameState { Normal, Paused, NotStarted };
}
