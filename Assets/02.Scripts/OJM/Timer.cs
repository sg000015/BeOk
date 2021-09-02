using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public bool timerOn = false;
    public float totalTime = 0f;

    private int minute = 0;
    private int second = 0;
    private int tic = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn)
        {
            totalTime += Time.deltaTime;
        }
        this.GetComponent<TMP_Text>().text = TimerCalc();
    }

    private string TimerCalc()
    {
        tic = (int)((totalTime % 1) * 100);

        second = (int)totalTime % 60;

        minute = (int)totalTime / 60;

        return $"{minute:00}" + ":" + $"{second:00}"; //+ " : " + tic;
    }

    public int[] GetTime()
    {
        int[] time = new int[3];
        time[0] = minute;
        time[1] = second;
        time[2] = tic;
        return time;
    }

}
