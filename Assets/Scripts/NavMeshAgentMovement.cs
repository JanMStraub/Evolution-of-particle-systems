using UnityEngine;

public class NavMeshAgentMovement : MonoBehaviour {
    private float _angularSpeed;
    private float _movementSpeed;
    private int _pathIndex;
    private float _avoidDistance;
    private Vector3 _actualTarget;
    private Vector3 _destination;
    private Vector3[] _path;
    private Transform _transform;


    private void Start() {
        _transform = this.GetComponent<Transform>();
        _movementSpeed = 1f;
        _angularSpeed  = 0.1f;
        _avoidDistance = 2f;
    }


    private void Update() {
        if(_path != null && _path.Length > 0 && !DestinationReached()) {
            if(TargetReached()) {
                _pathIndex++;
                if(!DestinationReached()) {
                    _actualTarget = _path[_pathIndex];
                }
            }
            steer();
            move();
            _movementSpeed = 1f;
        }
        
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
            Destroy(gameObject);
            return true;
        } else {
            return false;
        }
    }


    private void Move() {
        _transform.forward += (_actualTarget - _transform.position) * _angularSpeed;
        _transform.position += _transform.forward * _movementSpeed;
    }

    private void steer() {
        int critCounter = 0;
        float rotationWidth = 0;

        if(Physics.Raycast(_transform.position, Quaternion.Euler(0,-45,0) * _transform.forward, _avoidDistance, 1)) {
            rotationWidth += 1;
        }
        if(Physics.Raycast(_transform.position, Quaternion.Euler(0,-15,0) * _transform.forward, _avoidDistance, 1)) {
            rotationWidth += 2;
            critCounter += 1;
        }
        if(Physics.Raycast(_transform.position, Quaternion.Euler(0,15,0) * _transform.forward, _avoidDistance, 1)) {
            rotationWidth -= 2;
            critCounter += 1;
        }
        if(Physics.Raycast(_transform.position, Quaternion.Euler(0,45,0) * _transform.forward, _avoidDistance, 1)) {
            rotationWidth -= 1;
        }

        if((critCounter >= 2) && (Physics.Raycast(_transform.position, _transform.forward, _avoidDistance, 1<<1))) { //obstacels in way, no space left and right 
            _movementSpeed = 0;
        } else {
            _transform.forward = Quaternion.Euler(0,rotationWidth*30f,0) * _transform.forward;
        }
    }

    public void SetPath(Vector3[] path) {
        if(path != null && path.Length > 1) {
            _path = path;
            _pathIndex = 1;
            _destination = path[path.Length - 1];
            _actualTarget = path[1];
        }
    }


    public void SetSpeed(float speed) {
        _movementSpeed = speed;
    }
}