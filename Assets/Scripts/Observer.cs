using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour {
    public GameObject Agent;

    void Awake() {
        for (int i = 0; i < 9; i++) {
            Instantiate(Agent);
        }
    }
}
