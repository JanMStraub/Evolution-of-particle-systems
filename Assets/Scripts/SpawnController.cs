using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

class SpawnController : MonoBehaviour {

    private static SpawnController _spawnControllerInstance;
    private Student[] _studentList;
    private ClockManagement _cM;
    private NavMeshPath[,] _pathList;
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

            CalcPaths();

            //Test();

            ClockManagement.ClockManagementInstance.StartTime();
            _cM = GameObject.Find("SimulationHandler").GetComponent<ClockManagement>();

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

                    if(routePoints[0] == routePoints[1]) {
                        continue;
                    }

                    GameObject startDoor = FindDoor(routePoints[0]);
                    GameObject endDoor = FindDoor(routePoints[1]);

                    Vector3 spawnPoint = UnityEngine.Random.insideUnitSphere * 30f + startDoor.transform.position;
                    NavMeshHit hit;
                    NavMesh.SamplePosition (spawnPoint, out hit, 30f, 1);
                    spawnPoint = hit.position;

                    instantiatedAgent = (GameObject)Instantiate(agent, spawnPoint, transform.rotation);
                    int startIndex = NameToIndex(startDoor.name);
                    int endIndex = NameToIndex(endDoor.name);
                    //instantiatedAgent.GetComponent<NavMeshAgentController>().Activate(FindDoor(routePoints[1]).transform.position);
                    NavMeshPath path = _pathList[startIndex, endIndex];
                    if(path == null) {
                        Debug.Log("no path found");
                    }

                    //instantiatedAgent.GetComponent<NavMeshAgent>().SetPath(path);
                    instantiatedAgent.GetComponent<NavMeshAgentMovement>().SetPath(path.corners);
                    //instantiatedAgent.GetComponent<NavMeshAgent>().Stop(true);
                    //instantiatedAgent.GetComponent<NavMeshAgentController>().Activate(path);

                    if(path == null) {
                        throw new System.Exception("no path found");
                    }

                    /*
                    if(!instantiatedAgent.GetComponent<NavMeshAgent>().hasPath) {
                        Debug.Log(path.corners.Length);
                        Debug.Log(startIndex + "  " + endIndex);
                        instantiatedAgent.GetComponent<NavMeshAgent>().SetPath(path);
                    }
                    */

    
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
            /*
            if(studentsFinished > 100) {
                break;
            }
            */
            _cM.SetGo();

            _clockManagement.SetGo();
            Debug.Log(_clockManagement.GetCurrentlyCalculationPathList());
            yield return new WaitForSeconds(3f);
        }

    }

    private GameObject FindDoor(string tag) {
        GameObject[] doors = GameObject.FindGameObjectsWithTag(tag);
        float randomPosition = UnityEngine.Random.Range(0,doors.Length); //System.Collections.Random.Range(0f, doors.Length -1f);
        return doors[(int)randomPosition];
    }

    // TODO: Randomize door selection
    private Vector3[] FindClosestDoor(Student student) {
        
        List<GameObject> doorsWithinNextComplex = new List<GameObject>();
        Vector3[] closestDoors = new Vector3[2];
        float distance = 100000000;

        foreach (GameObject door in _doors) {
            if (int.Parse(door.tag) == student.GetNextLecture().building) {
                doorsWithinNextComplex.Add(door);
            }
        }

        if (doorsWithinNextComplex.Count != 0 
        && student.GetDoorsWithinCurrentComplex().Count != 0) {

            foreach (GameObject currentDoor in student.GetDoorsWithinCurrentComplex()) {
                foreach (GameObject nextDoor in doorsWithinNextComplex) {
                    float currentDistance = Vector3.Distance(currentDoor.transform.position, nextDoor.transform.position);
                    if (distance > currentDistance) {
                        closestDoors[0] = currentDoor.transform.position;
                        closestDoors[1] = nextDoor.transform.position;
                    }
                }
            }
        } else if (student.GetLectureIndex() == 0) { // First lecture of the day
            closestDoors[0] = student.GetSpawnPoint();

            foreach (GameObject nextDoor in doorsWithinNextComplex) {
                    float currentDistance = Vector3.Distance(student.GetSpawnPoint(), nextDoor.transform.position);
                    if (distance > currentDistance) {
                        closestDoors[1] = nextDoor.transform.position;
                    }
            }
        } else if (student.GetLectureIndex() >= student.lectureList.Count) { // All lectures finished
            closestDoors[1] = student.GetSpawnPoint();

            foreach (GameObject currentDoor in student.GetDoorsWithinCurrentComplex()) {
                    float currentDistance = Vector3.Distance(student.GetSpawnPoint(), currentDoor.transform.position);
                    if (distance > currentDistance) {
                        closestDoors[0] = currentDoor.transform.position;
                    }
            }
        } else {
            throw new System.Exception("illegal position");
        }
        return closestDoors;
    }

    private void CalcPaths() {
        _pathList = new NavMeshPath[_doors.Count,_doors.Count];
        int pathcount = 0;
        foreach(GameObject startDoor in _doors) {
            foreach(GameObject finishDoor in _doors) {
                
                /*
                if(startDoor.tag == finishDoor.tag) { //doors at same house
                    continue;
                }
                */
                
                agent.transform.position = startDoor.transform.position;
                NavMeshPath path = new NavMeshPath();
                //agent.GetComponent<NavMeshAgent>().CalculatePath(finishDoor.transform.position, path);

                NavMesh.CalculatePath(startDoor.transform.position, finishDoor.transform.position, 1, path);
                pathcount++;
                if(path.corners.Length < 1) {
                    Debug.Log(startDoor.name + "  " + finishDoor.name);
                }


                
                /*
                if(startDoor.name == "SpawnPoint (84)" || startDoor.name == "SpawnPoint (83)" || startDoor.name == "SpawnPoint (82)" || startDoor.name == "SpawnPoint (81)" || startDoor.name == "SpawnPoint (80)") {
                    for(int i=0; i<3; i++) {
                        GameObject instantiatedAgent = (GameObject)Instantiate(agent, startDoor.transform.position, transform.rotation);
                        instantiatedAgent.GetComponent<NavMeshAgentMovement>().SetPath(path.corners);
                    }
                }
                */

                if(path == null) {
                    throw new System.Exception("path is null");
                }
                
                

                _pathList[NameToIndex(startDoor.name), NameToIndex(finishDoor.name)] = path;
            }
        }
    }

    private int NameToIndex(string name) { //Get number of the door out of its name
        string number = name.Split(' ')[1];
        number = number.Substring(1, number.Length - 2);
        return int.Parse(number);
    }

    private void Test() {
        GameObject end = GameObject.Find("door (57)");
        GameObject start = GameObject.Find("SpawnPoint (84)");
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(start.transform.position, end.transform.position, 1, path);
        GameObject instantiatedAgent = (GameObject)Instantiate(agent, start.transform.position, transform.rotation);
        instantiatedAgent.GetComponent<NavMeshAgentMovement>().SetPath(path.corners);

    }

}
