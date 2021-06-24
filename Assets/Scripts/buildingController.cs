using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildingController : MonoBehaviour {

    [SerializeField]
    int _agentCounter = 0;

    private void OnTriggerEnter(Collider collision) {
        GameObject agent = collision.gameObject;
        _agentCounter++;
        
        Debug.Log(agent.GetInstanceID() + " entered building");
    }
}
