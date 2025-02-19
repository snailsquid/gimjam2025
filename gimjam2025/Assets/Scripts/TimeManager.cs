using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float CurrentTime { get; private set; }
    [SerializeField] float realToGameTime = 30;
    [SerializeField] TMP_Text timeText;
    float RealTime;
    bool isTimePlaying = false;
    public void UI(bool show)
    {
        timeText.transform.parent.gameObject.SetActive(show);
    }
    public void StartTime()
    {
        isTimePlaying = true;
    }
    public void RestartTime()
    {

        RealTime = 0;
    }
    public void PauseTime()
    {
        isTimePlaying = false;
    }
    void Start()
    {
        isTimePlaying = true;
    }
    void Update()
    {
        if (isTimePlaying)
        {
            RealTime += Time.deltaTime;
        }
        CurrentTime = RealTime * realToGameTime / 3600f;
        float minute = Mathf.Floor(CurrentTime);
        float second = (float)Math.Floor(CurrentTime % 1 * 100 * 0.6) % 60;
        timeText.text = $"Time Spent "+String.Format("{0:00}:{1:00}", minute, second);
    }
}
