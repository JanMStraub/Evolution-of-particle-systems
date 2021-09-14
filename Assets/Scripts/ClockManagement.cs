using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockManagement : MonoBehaviour {
    private static ClockManagement _ClockManagmentInstance;
    float time;
    float time_speed = 1f;
    Text time_text;

    public static ClockManagement ClockManagementInstance {
        get {return _ClockManagmentInstance;}
    }

    void Awake () {
        _ClockManagmentInstance = this;
    }

    void Start()
    {
        time = 0;
        time_speed = 1f;
        time_text = GameObject.Find("TimeDisplay").GetComponent<Text>();
    }

    void FixedUpdate()
    {
        if(time >= 1440)
        {
            time = 0;
        }
        time += (time_speed/50); //about 50 calls per second
        DisplayTime();
    }

    void DisplayTime()
    {
        time_text.text = this.GetTimeString();
    }

    public string GetTimeString()
    {
        string h_time;
        float hour = (int)time;
        float minute = (int)((time - hour) * 60);
        h_time = hour + ":" + minute;
        return h_time;
    }

    public float GetTime()
    {
        return time;
    }

    public float GetTimeSpeed()
    {
        return time_speed;
    }
}
