using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManagement : MonoBehaviour
{
    Light sun_light;

    float time;

    // Start is called before the first frame update
    void Start()
    {
        sun_light = this.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        time = GameObject.Find("SimulationHandler").GetComponent<ClockManagement>().GetTime();
        float intensity = (time*time - 24*time + 80)/-64;
        if(intensity > 0)
        {
            sun_light.intensity = intensity;

        }
        else
        {
            sun_light.intensity = 0;
        }
    }
}
