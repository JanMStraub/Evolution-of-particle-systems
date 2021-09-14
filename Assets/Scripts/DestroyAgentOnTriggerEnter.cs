using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAgentOnTriggerEnter : MonoBehaviour {

    void OnTriggerEnter(Collider collider){
        if (collider.tag == "Agent") {
            Destroy(collider.gameObject);
        }
    }
}
