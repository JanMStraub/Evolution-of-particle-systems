using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Observer : MonoBehaviour {

    [SerializeField] int agentCount = 5;

    GameObject _Agent;

    void Awake() {
        GameManager.OnGameStateChanced += GameManagerOnGameStateChanged;
    }

    void OnDestroy() {
        GameManager.OnGameStateChanced -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged (GameState state) {
         if (state == GameState.SpawnAgents) {
            Spawn();
        }
    }

    public void Spawn() {

        for (int i = 1; i < agentCount; i++) {
            _Agent = GameObject.FindGameObjectWithTag("Agent");
            Instantiate(_Agent);
        }

        GameManager.Instance.UpdateGameState(GameState.SetAgentCommute);
    }
}
