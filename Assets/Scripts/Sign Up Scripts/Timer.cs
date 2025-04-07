﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    private float timeRemaining = 100;
    public float TotalTimer;
    public bool timerIsRunning = false;
    private Text timerText;
    private void Start()
    {
        timeRemaining = TotalTimer;
        // Starts the timer automatically
        timerIsRunning = true;
        // timerText
    }

    private void OnEnable()
    {
        timeRemaining = TotalTimer;
        // Starts the timer automatically
        timerIsRunning = true;
        // timerText
        now = System.DateTime.Now;
    }
    public void ResetTimer()
    {
        timeRemaining = TotalTimer;
    }
    void AdjustTimer(float offset)
    {
        timeRemaining -= offset;
    }
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                float minutes = Mathf.FloorToInt(timeRemaining / 60);
                float seconds = Mathf.FloorToInt(timeRemaining % 60);
                this.GetComponent<Text>().text = minutes.ToString() + ":" + seconds.ToString();
                this.GetComponent<Text>().text = string.Format("{00:00}:{01:00}", minutes, seconds);


            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                this.GetComponent<Text>().text = timeRemaining.ToString();
                timerIsRunning = false;
            }
        }
    }
    System.DateTime now;
    System.TimeSpan buffr;
    void OnApplicationFocus(bool hasFocus)
    {

        if (hasFocus)
        {
            buffr = (System.DateTime.Now - now);
            AdjustTimer(buffr.Seconds);  // minus the difference 
            //not in background
        }
        else
        {
            now = System.DateTime.Now;   // assign time when in background
            //in the background
        }
    }
}