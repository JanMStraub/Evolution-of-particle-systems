using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshAgentController : MonoBehaviour {

    [SerializeField]
    Transform _destination;
    UnityEngine.AI.NavMeshAgent _agent;

    void Start() {
        _agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();

        if(_agent == null) {
            Debug.LogError("The nav mesh agent component is not attached to " + gameObject.name);
        } else {
            SetDestination();
        }
    }

    void Update() {
        SetDestination();
    }

    private void SetDestination() {
        if(_destination != null){
            Vector3 targetVector = _destination.transform.position;
            _agent.SetDestination(targetVector);
        }
    }
}
