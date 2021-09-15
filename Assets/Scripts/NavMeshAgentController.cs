using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class NavMeshAgentController : MonoBehaviour {

    public NavMeshAgent _agent;


    public void Activate(Vector3 destination) {
        _agent = this.GetComponent<NavMeshAgent>();
        
        _agent.SetDestination(destination);
    }
}
