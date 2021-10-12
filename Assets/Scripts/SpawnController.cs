using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

class SpawnController : MonoBehaviour {

    private static SpawnController _spawnControllerInstance;
    private Student[] _studentList;
    private ClockManagement _clockManagement;
    private static Vector3[] _path = new Vector3[0];


    [SerializeField] List<GameObject> _doors = new List<GameObject>();

    public GameObject agent;
    private LineRenderer lineRenderer;
    public NavMeshAgent navMeshAgent;
    public GameObject lineObject;
    public GameObject doorsParent;
    

    public static SpawnController SpawnControllerInstance {
        get {return _spawnControllerInstance;}
    }


    private void Awake() {
        GameManager.onGameStateChanced += GameManagerOnGameStateChanged;
        _spawnControllerInstance = this;
        
    }


    private void OnDestroy() {
        GameManager.onGameStateChanced -= GameManagerOnGameStateChanged;
    }


    private void GameManagerOnGameStateChanged (GameState state) {
         if (state == GameState.RunSimulation) {
            _studentList = StudentInitialisation.StudentInitialisationInstance.GetStudentList();

            foreach (Transform door in doorsParent.transform) {
                _doors.Add(door.gameObject);
            }

            ClockManagement.ClockManagementInstance.StartTime();
            _clockManagement = GameObject.Find("SimulationHandler").GetComponent<ClockManagement>();

            StartCoroutine(Spawn());
        }
    }
        /*

        234/235/236 zusammen als 234
        306/308/324/325/327/328/329/345/346/348 zusammen als 306
        304 Mensa
        700/720 zusammen als 700

        */

    private IEnumerator Spawn() {
        _studentList = CommuteController.CommuteControllerInstance.GetStudentList();
        int studentListSize = _studentList.Length;

        while (true) {
            int studentsFinished = 0;

            int gameTime = (int) ClockManagement.ClockManagementInstance.GetTime();
            GameObject instantiatedAgent = null;
            GameObject instantiatedLine = null;

            foreach (Student student in _studentList) {
                if(student.check(gameTime) == 1) {
                    string[] routePoints = student.RoutePoints();

                    Vector3 spawnPoint = UnityEngine.Random.insideUnitSphere * 50f + FindDoor(routePoints[0]);
                    NavMeshHit hit;
                    NavMesh.SamplePosition (spawnPoint, out hit, 50f, 1);
                    spawnPoint = hit.position;

                    instantiatedAgent = (GameObject)Instantiate(agent, spawnPoint, transform.rotation);
                    instantiatedAgent.GetComponent<NavMeshAgentController>().Activate(FindDoor(routePoints[1]), spawnPoint);
                    instantiatedLine = (GameObject)Instantiate(lineObject);

                    yield return new WaitUntil(() => !instantiatedAgent.GetComponent<NavMeshAgent>().pathPending);
                    _path = instantiatedAgent.GetComponent<NavMeshAgent>().path.corners;

                    if (_path != null && _path.Length > 1) {
                        instantiatedLine = (GameObject)Instantiate(lineObject);
                        instantiatedLine.GetComponent<DrawPath>().DrawPathOnFloor(_path);
                    }

                } else if(student.check(gameTime) == 2) {
                    studentsFinished++;
                }
            }

            _clockManagement.SetGo();
            Debug.Log(_clockManagement.GetCurrentlyCalculationPathList());
            yield return new WaitForSeconds(3f);
        }

    }

    private Vector3 FindDoor(string tag) {
        GameObject[] doors = GameObject.FindGameObjectsWithTag(tag);
        float randomPosition = UnityEngine.Random.Range(0,doors.Length); //System.Collections.Random.Range(0f, doors.Length -1f);
        return doors[(int)randomPosition].transform.position;
    }    
}
