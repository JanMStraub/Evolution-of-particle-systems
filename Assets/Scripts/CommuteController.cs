using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommuteController : MonoBehaviour {    
    private int _agentCounter = 0;

    private void OnTriggerEnter(Collider collision) {
        GameObject agent = collision.gameObject;
        _agentCounter++;
        
        Destroy(agent);
    }

    private void Timer() {

    }
}
