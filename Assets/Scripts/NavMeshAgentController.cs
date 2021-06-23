using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentController : MonoBehaviour {

    [SerializeField]
    Transform _workDestination, _homeDestination;
    NavMeshAgent _agent;
    AgentAgendaController _agentAgendaController;
    int _pauseTime = 5;
    Renderer _agentRenderer;

    void Start() {
        _agent = this.GetComponent<NavMeshAgent>();
        _agentRenderer = GetComponent<Renderer>();
        
        if(_agent == null) {
            Debug.LogError("The nav mesh agent component is not attached to " + gameObject.name);
        } 
        
        StartCoroutine(Commute());
    }

    IEnumerator Commute() {
        while(true) {
            // Agent work routine
            workRoutine();

            yield return new WaitUntil(()=>reachedDestination(_workDestination));
            yield return new WaitForSeconds(_pauseTime);

            // Agent home routine
            homeRoutine();
            
            yield return new WaitUntil(()=>reachedDestination(_homeDestination));
            yield return new WaitForSeconds(_pauseTime);
        }
    }

    void workRoutine() {
        Debug.Log(_agent.GetInstanceID() + " work routine");
        _agentRenderer.enabled = true;
        _agent.radius = 0.5f;
        _agent.isStopped = false;
        _agent.SetDestination(_workDestination.transform.position);
    }

    void homeRoutine() {
        Debug.Log(_agent.GetInstanceID() + " home routine");
        _agentRenderer.enabled = true;
        _agent.radius = 0.5f;
        _agent.isStopped = false;
        _agent.SetDestination(_homeDestination.transform.position);
    }

    bool reachedDestination(Transform destination) {
        float distance = Vector3.Distance(_agent.transform.position, destination.transform.position);

        if (distance < _agent.stoppingDistance) {
            Debug.Log(_agent.GetInstanceID() + " destination reached");
            _agentRenderer.enabled = false;
            _agent.radius = 0.0001f;
            _agent.isStopped = true;
            return true;
        }

        return false;
    }
}
