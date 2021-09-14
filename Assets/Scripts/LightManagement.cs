using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManagement : MonoBehaviour
{
    Light sun_light;

    float time;

    void Start () {
        time = ClockManagement.ClockManagementInstance.GetTime();
        sun_light = this.GetComponent<Light>();
    }

    void Update () {
        float intensity = (time*time - 24*time + 80)/-64;
        if (intensity > 0) {
            sun_light.intensity = intensity;

        } else {
            sun_light.intensity = 0;
        }
    }
}
