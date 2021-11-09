using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClockManagement : MonoBehaviour {

    private static ClockManagement _clockManagementInstance;
    private float _currentTime;
    private float _timeSpeed;
    
    public TMP_Text timeText;


    public static ClockManagement ClockManagementInstance {
        get {return _clockManagementInstance;}
    }


    void Awake() {
        _clockManagementInstance = this;
    }


    public void StartTime() {
        _currentTime = 460;
        _timeSpeed = 0.8f; // 0.8f
    }


    void FixedUpdate() {
        if (_currentTime >= 1440) {
            _currentTime = 0;
        }
        _currentTime += (_timeSpeed/50); //about 50 calls per second
        DisplayTime();
    }

    
    void DisplayTime() {
        timeText.text = this.GetTimeString();
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
}