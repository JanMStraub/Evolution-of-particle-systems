using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class NavMeshAgentController : MonoBehaviour {

    private int _studentId;

    private float _agentSpeed;

    private ClockManagement cM;

    public NavMeshAgent agent;


    public void Activate(Vector3 destination) {
        //_studentId = student.GetId();
        // agent.height(student.GetSize);
        // agent.speed(student.GetSpeed);
        agent.SetDestination(destination);
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
