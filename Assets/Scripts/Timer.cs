using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Include this to interact with UI elements

public class Timer : MonoBehaviour
{
    public Text timerText; // Reference to the Text component for displaying the time
    private float timeRemaining = 120f; // 2 minutes in seconds

    void Start()
    {
        if (timerText == null)
        {
            Debug.LogError("Timer Text component not set on the Timer script.");
        }
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime; // Decrease the time remaining
            UpdateTimerDisplay(timeRemaining); // Update the text display
        }
        else
        {
            timeRemaining = 0;
            UpdateTimerDisplay(timeRemaining); // Make sure to update the display one last time
            Debug.Log("Countdown finished!"); // Optionally perform an action when the countdown ends
        }
    }

    void UpdateTimerDisplay(float timeToDisplay)
    {
        timeToDisplay += 1; // Add 1 to ensure the last second is displayed correctly

        int minutes = Mathf.FloorToInt(timeToDisplay / 60); // Calculate the minutes left
        int seconds = Mathf.FloorToInt(timeToDisplay % 60); // Calculate the seconds left

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // Update the text display in "mm:ss" format
    }
}
