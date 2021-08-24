using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

class SpawnController : MonoBehaviour {

    int agentCount = 5;

    public float range = 30f;
    
    GameObject _Agent;


    void Start () {
        _Agent = GameObject.FindGameObjectWithTag("Agent");
        Debug.Log("SpawnAgents");
        Spawn();
    }


    bool RandomPoint(Vector3 center, float range, out Vector3 result) {

        for (int i = 1; i < agentCount; i++) {

            Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
            NavMeshHit hit;
            
            if (NavMesh.SamplePosition (randomPoint, out hit, 1.0f, NavMesh.AllAreas)) {
                result = hit.position;
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }


    void Spawn() {

        Vector3 point;
        
        for (int i = 1; i < agentCount; i++) {

            if (RandomPoint(_Agent.transform.position, range, out point)) {
                Instantiate(_Agent, point, transform.rotation);
            }
        }
        
        GameManager.GameManagerInstance.UpdateGameState(GameState.SetAgentCommute);
    }
}
