using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class NavMeshAgentController : MonoBehaviour {

    private NavMeshPath _path;
    private float _agentSpeed;
    private bool _isOnList = false;
    private ClockManagement clockManagement;

    public NavMeshAgent agent; 
    public LineRenderer line;
    public static Vector3[] path = new Vector3[0];

    public Vector3 destination;


    public void Activate(NavMeshPath inPath) {
        agent = this.GetComponent<NavMeshAgent>();
        line = this.GetComponent<LineRenderer>();

        agent.SetPath(inPath);
        //agent.SetDestination(destination); 
    }

    void Start() {
        agent = this.GetComponent<NavMeshAgent>();
        clockManagement = GameObject.Find("SimulationHandler").GetComponent<ClockManagement>();
        _agentSpeed = agent.speed;
    }

    void Update() {
        Color color;

        if(agent.pathPending) {
            color = Color.red;
            if (_isOnList == false) {
                clockManagement.AddCurrentlyCalculatingPathList(agent.GetInstanceID());
                _isOnList = true;
            }
            clockManagement.SetPause();
        } else {
            color = Color.green;
            clockManagement.RemoveCurrentlyCalculatingPathList(agent.GetInstanceID());
            _isOnList = false;
        }

        GetComponent<Renderer>().material.color = color;

        if(clockManagement.GetTimeSpeed() == 0) {
            agent.speed = 0;
        } else {
            agent.speed = _agentSpeed;
        }

        path = agent.path.corners;


        if (path != null && path.Length > 1) {
            line.positionCount = path.Length;
            for (int i = 0; i < path.Length; i++) {
                line.SetPosition(i, path[i]);
            }
        }
    }
}
