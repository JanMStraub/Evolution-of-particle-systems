using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

class Spawner : MonoBehaviour {

    [SerializeField] int agentCount = 5;

    [SerializeField] float range = 30f;
    GameObject _Agent;

    void Awake() {
        GameManager.OnGameStateChanced += GameManagerOnGameStateChanged;
        _Agent = GameObject.FindGameObjectWithTag("Agent");
    }

    void OnDestroy() {
        GameManager.OnGameStateChanced -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged (GameState state) {
         if (state == GameState.SpawnAgents) {
            Spawn();
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result) {
        for (int i = 0; i < agentCount; i++) {
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
        
        for (int i = 0; i < agentCount; i++) {

            if (RandomPoint(_Agent.transform.position, range, out point)) {
                Instantiate(_Agent, point, transform.rotation);
            }
        }
        
        _Agent.SetActive(false);
        GameManager.Instance.UpdateGameState(GameState.SetAgentCommute);
    }
}
