using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    float move_speed;
    float zoom_speed;
    
    float min_height;
    float max_height;
    
    float x_max;
    float z_max;
    
    Vector3 camera_start;
    
    // Start is called before the first frame update
    void Start()
    {
        move_speed = 1f;
        zoom_speed = 1f;
        
        min_height = 20f;
        max_height = 300f;
        
        x_max = 400f;
        z_max=400f;
        
        this.transform.position = GameObject.Find("Floor").transform.position;
        this.transform.position += new Vector3(0, (min_height + max_height)/2, 0);
        
        camera_start = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("w") && ((this.transform.position.z - camera_start.z) < z_max))
        {
            this.transform.position += (this.transform.up * move_speed);
        }
        if(Input.GetKey("a") && ((camera_start.x - this.transform.position.x) < x_max))
        {
            this.transform.position -= (this.transform.right * move_speed);
        }
        if(Input.GetKey("s") && ((camera_start.z - this.transform.position.z) < z_max))
        {
            this.transform.position -= (this.transform.up * move_speed);
        }
        if(Input.GetKey("d") && ((this.transform.position.x - camera_start.x) < x_max))
        {
            this.transform.position += (this.transform.right * move_speed);
        }
        if(Input.GetKey("r") && this.transform.position.y > min_height)
        {
            this.transform.position += (this.transform.forward * zoom_speed);
        }
        if(Input.GetKey("f") && this.transform.position.y < max_height)
        {
            this.transform.position -= (this.transform.forward * zoom_speed);
        }
    }
}
