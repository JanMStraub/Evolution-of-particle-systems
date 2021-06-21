using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAgendaController : MonoBehaviour {

    void Update() {
        
    }

    private void OnTriggerEnter(Collider collision) {
        GameObject agent = collision.gameObject;
        
        // agent.GetComponent<NavMeshAgentController>().enabled = false;
    }


}
