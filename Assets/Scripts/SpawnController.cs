using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

class SpawnController : MonoBehaviour {

    public int agentCount = 10;

    public float range = 30f;

    private static SpawnController _SpawnControllerInstance;
    
    public float spawnProgress;

    public bool isDone;
    
    private GameObject _Agent;
    
    public static SpawnController SpawnControllerInstance {
        get {return _SpawnControllerInstance;}
    }

    void Awake () {
        _SpawnControllerInstance = this;
    }

    void Start () {
        _Agent = GameObject.FindGameObjectWithTag("Agent");
        Debug.Log("SpawnAgents");
        Spawn();
    }


    void Spawn() {
        
        for (int i = 1; i < agentCount; i++) {

            Vector3 randomPoint = UnityEngine.Random.insideUnitSphere * range;
            NavMeshHit hit;

            NavMesh.SamplePosition (randomPoint,
                                    out hit,
                                    range,
                                    1);

            Vector3 point = hit.position;
            
            Instantiate(_Agent,
                        point,
                        transform.rotation);

            spawnProgress = ((float)i / (float)agentCount);
        }

        isDone = true;

        GameManager.GameManagerInstance.UpdateGameState(GameState.SetAgentCommute);
    }
}
