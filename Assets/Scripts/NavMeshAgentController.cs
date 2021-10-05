using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Burst;
using UnityEngine;
using UnityEngine.AI;
using Unity.Collections;
using UnityEngine.Experimental.AI;

public class NavMeshAgentController : MonoBehaviour {

    private Vector3 _destination;
    private NavMeshPath _path;
    private float _agentSpeed;
    private bool _isOnList = false;
    private ClockManagement clockManagement;

    public NavMeshAgent agent;
    public LineRenderer line;
    public static Vector3[] path = new Vector3[0];


    public void Activate(Vector3 destination) {
        line = this.GetComponent<LineRenderer>();
        this._destination = destination;

        foreach (GameObject agent in SpawnController.SpawnControllerInstance.AgentList) {
            _path = new NavMeshPath();
            NavMesh.CalculatePath(agent.transform.position, _destination, NavMesh.AllAreas, _path);
            agent.GetComponent<NavMeshAgent>().SetPath(_path);
        }

        // JobHandle jobHandle = SetDestinationAndCalculatePathJob(agent, _destination);
        // jobHandle.Complete();
        // agent.SetDestination(_destination); 
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

        // Draw path
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
/*
    private JobHandle SetDestinationAndCalculatePathJob(NavMeshAgent agent, Vector3 destination) {
        PathCalculationJob job = new PathCalculationJob (agent, destination);
        return job.Schedule();
    }
}

[BurstCompile]
public struct PathCalculationJob : IJob {

    public NativeList<NavMeshAgent> Agent = new NativeList<NavMeshAgent>(1, Allocator.TempJob);
    public NativeList<Vector3> Destination = new NativeList<Vector3>(1, Allocator.TempJob);

    public PathCalculationJob(NavMeshAgent agent, Vector3 destination) {
        this.Agent.Add(agent);
        this.Destination.Add(destination);
    }

    public void Execute() {            
        Agent[0].SetDestination(Destination[0]); 
    }
*/
}


public struct PathCalculationJob : IJobParallelFor {

    [ReadOnly] public NativeArray<float3> spawnArray;
    [ReadOnly] public NativeArray<float3> destinationArray;
    [ReadOnly] public NavMeshQuery navMeshQuery = new NavMeshQuery(NavMeshWorld.GetDefaultWorld(), Allocator.Persistent, 100);

    public void Execute(int index) {            
        // agent.GetComponent<NavMeshAgent>().SetDestination(destinationArray[index]);
        navMeshQuery.BeginFindPath(spawnArray[index], destinationArray[index]);
    }
    
}