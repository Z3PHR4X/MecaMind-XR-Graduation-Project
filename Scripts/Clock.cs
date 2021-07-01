using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text dateText;
    
    void Update()
    {
        String hour;
        if (System.DateTime.Now.Hour < 10)
        {
            hour = "0" + System.DateTime.Now.Hour.ToString();
        }
        else
        {
            hour = System.DateTime.Now.Hour.ToString();
        }

        String minute;
        if (System.DateTime.Now.Minute < 10)
        {
            minute = "0" + System.DateTime.Now.Minute.ToString();
        }
        else
        {
            minute = System.DateTime.Now.Minute.ToString();
        }
        
        timeText.text = hour + ":" + minute; 
        dateText.text = System.DateTime.Now.Day + "/" + System.DateTime.Now.Month + "/" + System.DateTime.Now.Year; 
    }
}
