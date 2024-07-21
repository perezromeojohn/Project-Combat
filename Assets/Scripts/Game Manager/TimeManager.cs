using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float timeElapsed; // Elapsed time in seconds
    private bool isPaused = false;
    public GameObject pauseScreen;
    public GameObject perkScreen;
    public GameObject topFrame;

    private bool gameEnded = false;

    void Start()
    {
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (!gameEnded)
        {
            timeElapsed += Time.deltaTime;
            UpdateTimerDisplay();
            EnsurePause();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // if perkScreen is active, close it
                if (!perkScreen.activeSelf)
                {
                    if (isPaused)
                    {
                        ResumeGame();
                        pauseScreen.SetActive(false);
                        topFrame.SetActive(true);
                    }
                    else
                    {
                        PauseGame();
                        pauseScreen.SetActive(true);
                        topFrame.SetActive(false);
                    }
                }
            }
        }
    }

    void UpdateTimerDisplay()
    {
        // Convert timeElapsed to minutes and seconds
        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);

        // Update the timer text
        timerText.text = string.Format("{0}:{1:00}", minutes, seconds);
    }

    public void EndGame()
    {
        gameEnded = true;
        Debug.Log("Game ended. Time elapsed: " + timeElapsed);
    }

    public float GetTimeElapsed()
    {
        return timeElapsed;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
    }

    private void EnsurePause()
    {
        if (isPaused)
        {
            PauseGame();
        }
    }
}