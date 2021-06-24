using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour {

    [SerializeField]
    int agentCount = 5;

    [SerializeField]
    GameObject _Agent;

    void Awake() {
        for (int i = 1; i < agentCount; i++) {
            Instantiate(_Agent);
            
        }
    }
}
