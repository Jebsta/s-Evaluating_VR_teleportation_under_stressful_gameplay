using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText; 
    
    // PUBLIC, damit der Manager die Zeit lesen kann
    public float currentTime = 0; 
    
    private bool timerIsRunning = false;

    private bool isVisible = true;

    public void SetVisibility(bool visible)
    {
        isVisible = visible;
    }

    public void StartRunning()
    {
        currentTime = 0;
        timerIsRunning = true;
    }

    // Neue Funktion, damit der Manager den Timer anhalten kann
    public void StopTimer()
    {
        timerIsRunning = false;
    }

    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        int milliseconds = Mathf.FloorToInt((currentTime * 1000f) % 1000f);

        //return string.Format("{0:00}:{1:00}", minutes, seconds);
        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }


    void Update()
    {
        if (timerIsRunning)
        {
            // Nur hochzählen
            currentTime += Time.deltaTime;

            // Nur Anzeigen
            float minutes = Mathf.FloorToInt(currentTime / 60);
            float seconds = Mathf.FloorToInt(currentTime % 60);
            if (isVisible)
                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            else
                timerText.text = "";   
        }
    }
}