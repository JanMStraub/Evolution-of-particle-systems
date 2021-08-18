using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    float move_speed;
    float zoom_speed;
    // Start is called before the first frame update
    void Start()
    {
        move_speed = 1f;
        zoom_speed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("w"))
        {
            this.transform.position += (this.transform.up * move_speed);
        }
        if(Input.GetKey("a"))
        {
            this.transform.position -= (this.transform.right * move_speed);
        }
        if(Input.GetKey("s"))
        {
            this.transform.position -= (this.transform.up * move_speed);
        }
        if(Input.GetKey("d"))
        {
            this.transform.position += (this.transform.right * move_speed);
        }
        if(Input.GetKey("r"))
        {
            this.transform.position += (this.transform.forward * zoom_speed);
        }
        if(Input.GetKey("f"))
        {
            this.transform.position -= (this.transform.forward * zoom_speed);
        }
    }
}
