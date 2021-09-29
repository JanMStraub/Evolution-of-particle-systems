using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class NavMeshAgentController : MonoBehaviour {

    private NavMeshPath _path;

    private float _agentSpeed;

    private ClockManagement cM;

    public NavMeshAgent agent;
    public LineRenderer line;


    public void Activate(Vector3 destination, Vector3 currentPosition) {
        agent = this.GetComponent<NavMeshAgent>();
        line = this.GetComponent<LineRenderer>();

        agent.SetDestination(destination); 

        _path = agent.path;

        line.SetPositions(_path.corners);
    }

    void Start() {
        agent = this.GetComponent<NavMeshAgent>();
        cM = GameObject.Find("SimulationHandler").GetComponent<ClockManagement>();
        _agentSpeed = agent.speed;
    }

    void Update() {
        Color color;

        if(agent.pathPending) {
            color = Color.red;
            cM.SetPause();
        } else {
            color = Color.green;
        }

        GetComponent<Renderer>().material.color = color;

        if(cM.GetTimeSpeed() == 0) {
            agent.speed = 0;
        } else {
            agent.speed = _agentSpeed;
        }
    }

    public int GetStudentId() {
        return _studentId;
    }
}
