using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class ClockManagement : MonoBehaviour {

    private static ClockManagement _clockManagementInstance;
    private float _currentTime;
    private float _deltaTime = 0f;
    private int _frameCount = 0;
    // Used for benchmarking
    private Dictionary<int, int> _fpsDict = new Dictionary<int, int>(); 

    [SerializeField] private float _timeSpeed;
    
    public TMP_Text timeText;


    // Instance for reference during run time
    public static ClockManagement ClockManagementInstance {
        get {return _clockManagementInstance;}
    }


    void Awake() {
        _clockManagementInstance = this;
    }


    public void StartTime() {
        _currentTime = 460; // 7:40
        // If time should work disconnected from FPS use 0.8f as value
        _timeSpeed = SimulationSettings.timeSpeed; 
    }


    private void Update() {
        _deltaTime += Time.unscaledDeltaTime;
        _frameCount++;

        if (_currentTime >= 1440) {
            _currentTime = 0;
            
            string path = Directory.GetCurrentDirectory();
            using (StreamWriter file = new StreamWriter(path + "/default.txt")) {
                foreach (var entry in _fpsDict)
                    file.WriteLine("{0} {1}", entry.Key, entry.Value); 
            }
        }
        _currentTime += (_timeSpeed/50); // 50 calls per second
        DisplayTime();

        
        int fps = (int)(_frameCount / _deltaTime);

        if (!_fpsDict.ContainsKey((int)_currentTime))
            _fpsDict.Add((int)(_currentTime), fps);
        

        // Reset variables
        _deltaTime = 0f;
        _frameCount = 0;
    }

    
    private void DisplayTime() {
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