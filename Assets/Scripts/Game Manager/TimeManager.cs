using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public string targetTime = "1:00"; // Target time in the format "3:00"
    private float timeElapsed; // Elapsed time in seconds
    private float targetSeconds; // Target time in seconds

    void Start()
    {
        // Convert targetTime to seconds
        targetSeconds = ConvertTimeToSeconds(targetTime);
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (timeElapsed < targetSeconds)
        {
            // Add the time passed since the last frame
            timeElapsed += Time.deltaTime;

            // Update the timer display
            UpdateTimerDisplay();
        }
        else
        {
            // Time's up, do something
            Debug.Log("Time's up!");
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

    float ConvertTimeToSeconds(string time)
    {
        string[] timeParts = time.Split(':');
        int minutes = int.Parse(timeParts[0]);
        int seconds = int.Parse(timeParts[1]);
        return minutes * 60 + seconds;
    }

    public float GetTimeElapsed()
    {
        return timeElapsed;
    }

    public float GetTargetTimeInSeconds()
    {
        return targetSeconds;
    }
}
