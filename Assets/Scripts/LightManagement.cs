using UnityEngine;

public class LightManagement : MonoBehaviour {

    private Light _sunLight;
    private float _time;


    void Start() {
        _time = ClockManagement.ClockManagementInstance.GetTime();
        _sunLight = this.GetComponent<Light>();
    }


    void Update() {
        float intensity = (_time*_time - 1750f*_time + 554400f)/-211225f;
        
        if (intensity > 0) {
            _sunLight.intensity = intensity;
        } else {
            _sunLight.intensity = 0;
        }
    }
}
