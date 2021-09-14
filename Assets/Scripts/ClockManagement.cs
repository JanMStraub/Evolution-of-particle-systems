using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockManagement : MonoBehaviour {

    private static ClockManagement _clockManagmentInstance;
    private float _currentTime;
    private float _timeSpeed = 1f;
    private Text _timeText;

    public GameObject timeTextGameObject;


    public static ClockManagement ClockManagementInstance {
        get {return _clockManagmentInstance;}
    }


    void Awake() {
        _clockManagmentInstance = this;
    }


    public void StartTime() {
        _currentTime = 450;
        _timeSpeed = 1f;
        //_timeText = GameObject.Find("TimeDisplay").GetComponent<TextMeshPro>();

    }


    void FixedUpdate() {
        if (_currentTime >= 1440) {
            _currentTime = 0;
        }
        _currentTime += (_timeSpeed/50); //about 50 calls per second
        DisplayTime();
    }

    
    void DisplayTime() {
        _timeText.text = this.GetTimeString();
    }
    

    public string GetTimeString() {
        string h_time;
        float hour = (int)(_currentTime/60f);
        float minute = (int)(_currentTime - (hour*60f));
        h_time = hour + ":" + minute;
        return h_time;
    }


    public float GetTime() {
        return _currentTime;
    }


    public float GetTimeSpeed() {
        return _timeSpeed;
    }
}
