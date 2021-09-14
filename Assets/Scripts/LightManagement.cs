using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManagement : MonoBehaviour {

    private Light _sunLight;
    float _time;


    void Start() {
        _time = ClockManagement.ClockManagementInstance.GetTime();
        _sunLight = this.GetComponent<Light>();
    }


    void Update() {
        float intensity = (_time*_time - 24*_time + 80)/-64;
        
        if (intensity > 0) {
            _sunLight.intensity = intensity;
        } else {
            _sunLight.intensity = 0;
        }
    }
}
