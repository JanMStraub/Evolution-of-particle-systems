using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    float _move_speed;
    float _zoom_speed;
    
    float _min_height;
    float _max_height;
    
    float _x_max;
    float _z_max;
    
    Vector3 _camera_start;
    
    // Start is called before the first frame update
    void Start() {
        _move_speed = 1f;
        _zoom_speed = 1f;
        
        _min_height = 20f;
        _max_height = 400f;
        
        _x_max = 400f;
        _z_max=550f;
        
        this.transform.position = GameObject.Find("Floor").transform.position;
        this.transform.position += new Vector3(0, (_min_height + _max_height)/2, 0);
        this.transform.localRotation = Quaternion.Euler(90,270,0);
        
        _camera_start = this.transform.position;
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKey("w") && ((this.transform.position.z - _camera_start.z) < _z_max)) {
            this.transform.position += (this.transform.up * _move_speed);
        }
        if(Input.GetKey("a") && ((_camera_start.x - this.transform.position.x) < _x_max)) {
            this.transform.position -= (this.transform.right * _move_speed);
        }
        if(Input.GetKey("s") && ((_camera_start.z - this.transform.position.z) < _z_max)) {
            this.transform.position -= (this.transform.up * _move_speed);
        }
        if(Input.GetKey("d") && ((this.transform.position.x - _camera_start.x) < _x_max)) {
            this.transform.position += (this.transform.right * _move_speed);
        }
        if(Input.GetKey("r") && this.transform.position.y > _min_height) {
            this.transform.position += (this.transform.forward * _zoom_speed);
        }
        if(Input.GetKey("f") && this.transform.position.y < _max_height) {
            this.transform.position -= (this.transform.forward * _zoom_speed);
        }
    }
}
