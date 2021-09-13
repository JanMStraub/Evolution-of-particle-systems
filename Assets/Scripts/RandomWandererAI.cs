using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomWandererAI : MonoBehaviour {


    public NavMeshAgent navMeshAgent;
    public Vector3 positon;


    void Update () {

        navMeshAgent.SetDestination(RandomNavSphere(positon, 25, -1));
    }

public static Vector3 RandomNavSphere (Vector3 origin, float distance, int layermask) {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
           
            randomDirection += origin;
           
            NavMeshHit navHit;
           
            NavMesh.SamplePosition (randomDirection, out navHit, distance, layermask);
           
            return navHit.position;
        }
}
