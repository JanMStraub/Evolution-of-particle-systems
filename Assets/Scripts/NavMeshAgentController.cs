using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class NavMeshAgentController : MonoBehaviour {

    // [SerializeField] int _pauseTime = 5;

    [SerializeField] Transform _workDestination, _homeDestination, _lunchDestination;
    NavMeshAgent _agent;
    Renderer _agentRenderer;

    ClockManagement cm;
    float time_speed;


    public Student _student;

    //public LectureList myLectureList = new LectureList();

    void Activate() {

        _agent = this.GetComponent<NavMeshAgent>();
        _agentRenderer = GetComponent<Renderer>();

        cm = GameObject.Find("SimulationHandler").GetComponent<ClockManagement>();
        _lunchDestination = GameObject.FindGameObjectWithTag("FoodDoor").transform;

        _agent.speed =(50 * (cm.GetTimeSpeed()*20));
        _agent.acceleration = 100;
        _agent.angularSpeed = 3000;

        if(_agent == null) {
            Debug.LogError("The nav mesh agent component is not attached to " + gameObject.name);
        } 
        
        GameManager.GameManagerInstance.UpdateGameState(GameState.RunSimulation);
        //StartCoroutine(Commute());
    }

    
    void Update()
    {
        //morning
        if((int)cm.GetTime() == 8)
        {
            movingTowards(_workDestination.position);
        }

        //lunch
        if((int)cm.GetTime() == 13)
        {
            movingTowards(_lunchDestination.position);
        }

        //afternoon
        if((int)cm.GetTime() == 14)
        {
            movingTowards(_workDestination.position);
        }

        //evening
        if((int)cm.GetTime() == 18)
        {
            movingTowards(_homeDestination.position);
        }
    }
    

    /*
    IEnumerator Commute() {
        while(true) {
            // Agent work routine
            if(cm.GetTime() == 8)
            {
                workRoutine();
                yield return new WaitUntil(()=>reachedDestination(_workDestination));

            }


            // Agent home routine
            if(cm.GetTime() == 18)
            {
                homeRoutine();
                yield return new WaitUntil(()=>reachedDestination(_homeDestination));

            }
            
        }
    }
    */
    

    void movingTowards(Vector3 position) {
        // Debug.Log(_agent.GetInstanceID() + " work routine");
        _agentRenderer.enabled = true;
        _agent.radius = 0.5f;
        _agent.isStopped = false;
        _agent.SetDestination(position);
    }

    /*
    void homeRoutine() {
        // Debug.Log(_agent.GetInstanceID() + " home routine");
        _agentRenderer.enabled = true;
        _agent.radius = 0.5f;
        _agent.isStopped = false;
        _agent.SetDestination(_homeDestination.transform.position);
    }
    */

    bool reachedDestination(Transform destination) {
        float distance = Vector3.Distance(_agent.transform.position, destination.transform.position);

        if (distance < _agent.stoppingDistance) {
            // Debug.Log(_agent.GetInstanceID() + " destination reached");
            _agentRenderer.enabled = false;
            _agent.radius = 0.000001f; // might find a better solution
            _agent.isStopped = true;
            return true;
        }

        return false;
    }

    public void setWorkDestination(GameObject destination) => _workDestination = destination.transform 
        ?? throw new System.ArgumentNullException(nameof(destination));

    public void setHomeDestination(GameObject destination) => _homeDestination = destination.transform
        ?? throw new System.ArgumentNullException(nameof(destination));
}
