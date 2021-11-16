using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentMovement : MonoBehaviour {
    private float _angularSpeed;
    private float _movementSpeed;
    private int _pathIndex;
    private float _avoidDistance;
    private Vector3 _actualTarget;
    private Vector3 _destination;
    private Vector3[] _path;
    private Transform _transform;

    public GameObject lineObject;
    public bool avoidEachOther;

    void Start() {
        avoidEachOther = SimulationSettings.agentAvoidance;
        _transform = this.GetComponent<Transform>();
        if(!avoidEachOther) {
            this.GetComponent<Collider>().enabled = false;
        }
    }

    void Update() {
        if(_path != null && _path.Length > 0 && !DestinationReached()) {
            if(TargetReached() || NextVisible()) {
                _pathIndex++;
                if(!DestinationReached()) {
                    _actualTarget = _path[_pathIndex];
                }
            }
            LookAhead();
            if(avoidEachOther) {
                Steer();
            }
            Move();
            _movementSpeed = 1f;
        }
        
    }

    private bool TargetReached() {
        float distance = (_transform.position.x - _actualTarget.x) * (_transform.position.x - _actualTarget.x) +
                         (_transform.position.y - _actualTarget.y) * (_transform.position.y - _actualTarget.y) + 
                         (_transform.position.z - _actualTarget.z) * (_transform.position.z - _actualTarget.z);
        if(distance < 25) {
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

    private void Steer() {
        float rotationWidth = 0;
        int collisions = 0;
        
        for(float i = -5; i<=5; i++) {
            if(Physics.Raycast(_transform.position, Quaternion.Euler(0,i*18,0) * _transform.forward, _avoidDistance, 1)) {
                collisions++;
                if(i<0) {
                    rotationWidth += (-18+i);
                } else {
                    rotationWidth += -(18-i);
                }
            }
        }
        if(collisions < 3) {
            _transform.forward = Quaternion.Euler(0,rotationWidth*5f,0) * _transform.forward; //adjust looking direction
        } else {
            _movementSpeed = 0.5f;
        }

        if(Physics.Raycast(_transform.position, _transform.forward, _avoidDistance, 2)) { // Obstacels in way, dont walk into them
            _movementSpeed = 0.1f;
        }
    }

    private void LookAhead() {
        if(Physics.Raycast(_transform.position, _actualTarget - _transform.position, 20, 1<<6)) { // layer 6 for navmeshobstacles
            NavMeshPath path = new UnityEngine.AI.NavMeshPath();
            GameObject instantiatedLine = null;
            NavMesh.CalculatePath(_transform.position, _destination, 1, path);
            SetPath(path.corners);
            
            if(!SpawnController.SpawnControllerInstance.alreadyUsedPaths.ContainsValue(path)) {
                instantiatedLine = (GameObject)Instantiate(lineObject);
                instantiatedLine.GetComponent<DrawPath>().DrawPathOnFloor(path.corners);
                SpawnController.SpawnControllerInstance.alreadyUsedPaths.Add(SpawnController.SpawnControllerInstance.pathID, path);
                SpawnController.SpawnControllerInstance.instantiatedLineList.Add(SpawnController.SpawnControllerInstance.pathID, instantiatedLine);
                SpawnController.SpawnControllerInstance.pathID++;
            } else {
                foreach (var pair in SpawnController.SpawnControllerInstance.alreadyUsedPaths) {
                    if (pair.Value == path) {
                        SpawnController.SpawnControllerInstance.instantiatedLineList[pair.Key].GetComponent<DrawPath>().ChangeWidthOfLine();
                    }
                }
            }
        }
    }

    private bool NextVisible() {
        return false;
    }

    public void SetPath(Vector3[] path) {
        if(path != null && path.Length > 1) {
            _path = path;
            _pathIndex = 1;
            _destination = path[path.Length - 1];
            _actualTarget = path[1];
        }
    }

    public void SetPersonality(float[] personality) { // We could implement that in the student initialisation
        this._movementSpeed = personality[0];
        this._angularSpeed = personality[1];
        this._avoidDistance = personality[2];
    }
}
