using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentController : MonoBehaviour {

    [SerializeField]
    Transform _destination;
    NavMeshAgent _agent;
    AgentAgendaController _agentAgendaController;
    int interval = 10;

    void Start() {
        _agent = this.GetComponent<NavMeshAgent>();

        if(_agent == null) {
            Debug.LogError("The nav mesh agent component is not attached to " + gameObject.name);
        } 
    }

    void Update() {
        if (Time.frameCount % interval == 0) {
            _agent.SetDestination(_destination.transform.position);
            if (reachedDestination()) {
                Timer(3.0f);
            }
        }
    }

    bool reachedDestination() {
        if (_agent.remainingDistance < _agent.stoppingDistance) {
            _agent.isStopped = true;
            return true;
        }
        return false;
    }

    void Timer(float time) {
        float targetTime = time;
        targetTime -= Time.deltaTime;

        if (targetTime <= 0.0f) {
            print("Test");
        }
    }
}
