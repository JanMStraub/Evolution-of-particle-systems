using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Spawner : MonoBehaviour {

    [SerializeField] int agentCount = 5;
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

    void Spawn() {

        for (int i = 0; i < agentCount; i++) {
            Instantiate(_Agent);
        }
        
        _Agent.SetActive(false);
        GameManager.Instance.UpdateGameState(GameState.SetAgentCommute);
    }
}
