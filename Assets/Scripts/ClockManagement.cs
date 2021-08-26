using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockManagement : MonoBehaviour
{
    float time;
    float time_speed;

    Light sun_light;

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        time_speed = 0.1f;
        sun_light = this.GetComponent<Light>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(time >= 24)
        {
            time = 0;
        }
        time += (time_speed/50); //about 50 calls per second

        AdjustLight(time);
    }

    void AdjustLight(float time)
    {
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

    public float GetTime()
    {
        return time;
    }

    public float GetTimeSpeed()
    {
        return time_speed;
    }
}
