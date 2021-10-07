using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Pathfinding;

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

        if (AstarPath.active == null) return;

        foreach (GameObject agent in SpawnController.SpawnControllerInstance.AgentList) {
            var p = ABPath.Construct(agent.transform.position, _destination);
            
            AstarPath.StartPath (p);
            
        }
        
        NativeArray<float3> spawnArray = new NativeArray<float3>(SpawnController.SpawnControllerInstance.AgentList.Count, Allocator.TempJob);
        NativeArray<float3> destinationArray = new NativeArray<float3>(SpawnController.SpawnControllerInstance.AgentList.Count, Allocator.TempJob);

        for (int i = 0; i < SpawnController.SpawnControllerInstance.AgentList.Count; i++) {
            float3 begin = new float3 (SpawnController.SpawnControllerInstance.AgentList[i].transform.position.x, SpawnController.SpawnControllerInstance.AgentList[i].transform.position.y, SpawnController.SpawnControllerInstance.AgentList[i].transform.position.z);
            float3 end = new float3 (_destination.x, _destination.y, _destination.z);
            
            spawnArray[i] = begin;
            destinationArray[i] = end;
        }  

        
        PathCalculationJob pathCalculationJob = new PathCalculationJob {
            spawnArray = spawnArray,
            destinationArray = destinationArray,
        };
        
        JobHandle jobHandle = pathCalculationJob.Schedule(SpawnController.SpawnControllerInstance.AgentList.Count, 100);
        jobHandle.Complete();

        for (int i = 0; i < SpawnController.SpawnControllerInstance.AgentList.Count; i++) {
            SpawnController.SpawnControllerInstance.AgentList[i].transform.position = spawnArray[i];
            _destination = destinationArray[i];
        }

        spawnArray.Dispose();
        destinationArray.Dispose();
        
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
}


public struct PathCalculationJob : IJobParallelFor {
    
    [ReadOnly] public NativeArray<float3> spawnArray;
    [ReadOnly] public NativeArray<float3> destinationArray;
    
    public void Execute(int index) {            
        // agent.GetComponent<NavMeshAgent>().SetDestination(destinationArray[index]);
        var p = ABPath.Construct(spawnArray[index], destinationArray[index]);
        AstarPath.StartPath (p);
    }
} 
