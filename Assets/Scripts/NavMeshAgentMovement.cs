using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshAgentMovement : MonoBehaviour
{
    private float _angularSpeed;
    private float _movementSpeed;
    private int _pathIndex;
    private Vector3 _actualTarget;
    private Vector3 _destination;
    private Vector3[] _path;
    private Transform _transform;

    void Start()
    {
        _transform = this.GetComponent<Transform>();
        _movementSpeed = 0.4f;
        _angularSpeed  = 0.02f;
    }

    void Update()
    {
        if(_path != null && _path.Length > 0 && !DestinationReached()) {
            if(TargetReached()) {
                _pathIndex++;
                if(!DestinationReached()) {
                    _actualTarget = _path[_pathIndex];
                }
            }
            move();
        }
        
    }

    private Vector3[] GetPath() {
        return _path;
    }

    private Vector3 GetDestination() {
        return _destination;
    }

    private bool TargetReached() {
        float distance = (_transform.position.x - _actualTarget.x) * (_transform.position.x - _actualTarget.x) +
                         (_transform.position.y - _actualTarget.y) * (_transform.position.y - _actualTarget.y) + 
                         (_transform.position.z - _actualTarget.z) * (_transform.position.z - _actualTarget.z);
        if(distance < 9) {
            return true;
        } else {
            return false;
        }
    }

    private bool DestinationReached() {
        if(_pathIndex == _path.Length) {
            return true;
        } else {
            return false;
        }
    }

    private void move() {
        _transform.forward += (_actualTarget - _transform.position) * _angularSpeed;
        _transform.position += _transform.forward * _movementSpeed;
    }

    public void SetPath(Vector3[] path) {
        if(path != null && path.Length > 1) {
            _path = path;
            _pathIndex = 1;
            _destination = path[path.Length - 1];
            _actualTarget = path[1];
        }
    }
}
