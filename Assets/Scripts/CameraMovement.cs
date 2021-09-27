using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    private float _moveSpeed;
    private float _zoomSpeed;
    private float _minHeight;
    private float _maxHeight;
    private float _xMax;
    private float _zMax;
    private Vector3 _cameraStart;


    private void Start() {
        _moveSpeed = 3f;
        _zoomSpeed = 1f;
        
        _minHeight = 20f;
        _maxHeight = 400f;
        
        _xMax = 400f;
        _zMax=550f;
        
        this.transform.position = GameObject.Find("Floor").transform.position;
        this.transform.position += new Vector3(0, (_minHeight + _maxHeight)/2, 0);
        this.transform.localRotation = Quaternion.Euler(90,270,0);
        
        _cameraStart = this.transform.position;
    }

 
    private void Update() {
        if (Input.GetKey("w") && ((this.transform.position.z - _cameraStart.z) < _zMax)) {
            this.transform.position += (this.transform.up * _moveSpeed);
        }
        if (Input.GetKey("a") && ((_cameraStart.x - this.transform.position.x) < _xMax)) {
            this.transform.position -= (this.transform.right * _moveSpeed);
        }
        if (Input.GetKey("s") && ((_cameraStart.z - this.transform.position.z) < _zMax)) {
            this.transform.position -= (this.transform.up * _moveSpeed);
        }
        if (Input.GetKey("d") && ((this.transform.position.x - _cameraStart.x) < _xMax)) {
            this.transform.position += (this.transform.right * _moveSpeed);
        }
        if (Input.GetKey("r") && this.transform.position.y > _minHeight) {
            this.transform.position += (this.transform.forward * _zoomSpeed);
        }
        if (Input.GetKey("f") && this.transform.position.y < _maxHeight) {
            this.transform.position -= (this.transform.forward * _zoomSpeed);
        }
    }
}
