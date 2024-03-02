using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public string initialTime = "3:00"; // Initial time in the format "3:00"
    private float timeRemaining; // Remaining time in seconds

    void Start()
    {
        // Convert initialTime to seconds
        timeRemaining = ConvertTimeToSeconds(initialTime);
        UpdateTimerDisplay();
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            // Subtract the time passed since the last frame
            timeRemaining -= Time.deltaTime;

            // Update the timer display
            UpdateTimerDisplay();

            // Check if time has run out
            if (timeRemaining <= 0)
            {
                // Time's up, do something
                Debug.Log("Time's up!");
            }
        }
    }

    void UpdateTimerDisplay()
    {
        // Convert timeRemaining to minutes and seconds
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

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
}
