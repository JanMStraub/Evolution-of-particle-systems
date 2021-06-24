using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommuteController : MonoBehaviour {    

    [SerializeField]
    GameObject[] _homeList;

    [SerializeField]
    GameObject[] _workList;

    [SerializeField]
    GameObject[] _agentList;

    void Awake() {

        // Get list of agents
        if (_agentList == null)
            _agentList = GameObject.FindGameObjectsWithTag("Agent");
            Debug.Log("Agent list created");

        // Assign home to agent
        if (_homeList == null)
            _homeList = GameObject.FindGameObjectsWithTag("Home");
            Debug.Log("Home list created");

        foreach (GameObject agent in _agentList) {
            int homeNumber = Random.Range(0, _homeList.Length);
            agent.GetComponent<NavMeshAgentController>()
                 .setHomeDestination(_homeList[homeNumber]);
            Debug.Log("Home assigned to " + agent.GetInstanceID());
        }

        // Assign work to agent
        if (_workList == null)
            _workList = GameObject.FindGameObjectsWithTag("Work");
            Debug.Log("Work list created");

        foreach (GameObject agent in _agentList) {
            int workNumber = Random.Range(0, _workList.Length);
            agent.GetComponent<NavMeshAgentController>()
                 .setWorkDestination(_workList[workNumber]);
            Debug.Log("Work assigned to " + agent.GetInstanceID());
        } 
    }
}
