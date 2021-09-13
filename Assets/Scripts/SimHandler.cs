using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimHandler : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent navMeshAgent;
    public GameObject Agent;
    public Vector3 point;
    public Vector3 positon;

    int i = 0;

    void Start () {
        Spawn();
    }

    void Spawn () {
        for (int i = 1; i < 6; i++) {
            
            Instantiate(Agent, point, transform.rotation);
        }
    }

}
