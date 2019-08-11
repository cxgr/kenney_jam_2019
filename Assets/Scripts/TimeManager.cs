using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public bool paused;

    public float shiftTime = 180f;

    public float timeLeft = 180f;
    
    public void SetPaused(bool on)
    {
        Time.timeScale = on ? 0f : 1f;
        paused = on;
    }

    private bool isRunning;
    public void StartTimer()
    {
        timeLeft = shiftTime;
        isRunning = true;
        UpdateTimeIndicator();
    }

    private void Update()
    {
        if (isRunning)
        {
            if (timeLeft <= 0f)
            {
                SingletonUtils<SessionManager>.Instance.FinishByTimeOut();
                isRunning = false;
            }
            
            timeLeft -= Time.deltaTime;
            UpdateTimeIndicator();
        }
    }

    void UpdateTimeIndicator()
    {
        var secondsTotal = Mathf.RoundToInt(timeLeft);
        var minutes = secondsTotal / 60;
        var seconds = secondsTotal % 60;
        
        SingletonUtils<UiManager>.Instance.UpdateTimeIndicator(
            $"{minutes.ToString()}:{seconds.ToString("00")}");
    }
}