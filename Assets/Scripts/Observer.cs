using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Observer : MonoBehaviour {

    [SerializeField]
    int agentCount = 5;

    GameObject _Agent;

    void Spawn() {
        for (int i = 1; i < agentCount; i++) {
            _Agent = GameObject.FindGameObjectWithTag("Agent");
            Instantiate(_Agent);
            
        }
    }
}
