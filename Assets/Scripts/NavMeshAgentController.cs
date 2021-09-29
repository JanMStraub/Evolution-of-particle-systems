using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class NavMeshAgentController : MonoBehaviour{

    private NavMeshPath _path;

    public NavMeshAgent agent;
    public LineRenderer line;


    public void Activate(Vector3 destination, Vector3 currentPosition) {
        agent = this.GetComponent<NavMeshAgent>();
        line = this.GetComponent<LineRenderer>();

        agent.SetDestination(destination); 

        _path = agent.path;

        line.SetPositions(_path.corners);
    }
}
