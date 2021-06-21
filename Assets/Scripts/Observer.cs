using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour {
    public GameObject Agent;
    public int agentCount = 5;

    void Awake() {
        for (int i = 1; i < agentCount; i++) {
            Instantiate(Agent);
            
        }
    }
}
