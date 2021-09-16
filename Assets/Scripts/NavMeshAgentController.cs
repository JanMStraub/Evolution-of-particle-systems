using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class NavMeshAgentController : MonoBehaviour {

    private int _studentId;

    public NavMeshAgent agent;


    public void Activate(Vector3 destination, Student student) {
        _studentId = student.GetId();
        agent = this.GetComponent<NavMeshAgent>();
        // agent.height(student.GetSize);
        // agent.speed(student.GetSpeed);
        agent.SetDestination(destination);
    }

    public int GetStudentId() {
        return _studentId;
    }
}
