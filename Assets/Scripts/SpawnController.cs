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

    [SerializeField] List<GameObject> _doors = new List<GameObject>();

    public GameObject agent;
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
                door.GetComponent<DestroyAgentOnTriggerEnter>().enabled = false;
            }

            CalcPaths();

            ClockManagement.ClockManagementInstance.StartTime();
            _cM = GameObject.Find("SimulationHandler").GetComponent<ClockManagement>();

            foreach (GameObject door in _doors) {
                door.GetComponent<DestroyAgentOnTriggerEnter>().enabled = true;
            }

            StartCoroutine(Spawn2());
        }
    }
        /*

        234/235/236 zusammen als 234
        306/308/324/325/327/328/329/345/346/348 zusammen als 306
        304 Mensa
        700/720 zusammen als 700


        Mensagebäude noch einfügen und Mittagspause reinbringen !ACHTUNG: Teilweise Vorlesungen den ganzen Tag -> Ausnahmen?

        */

    /*
    IEnumerator Spawn() {
        int studentsFinished = 0;
        _studentList = CommuteController.CommuteControllerInstance.GetStudentList();
        int studentListSize = _studentList.Length;

        while (studentsFinished < studentListSize) {
            int gameTime = (int) ClockManagement.ClockManagementInstance.GetTime();
            GameObject instantiatedAgent = null;

            foreach (Student student in _studentList) {
                // Exception ?
                if ((student.lectureList.Count > 0) 
                && (student.GetCurrentlyEnRoute() == false) 
                && (student.GetDayFinished() == false)
                && (student.GetCurrentLecture().GetEndInMinutes() > gameTime)) {

                    int nextLectureBegin = student.GetCurrentLecture().GetStartInMinutes();

                    if ((student.GetLectureIndex() == 0) 
                    && (gameTime >= nextLectureBegin - 15)) { // First lecture of the day

                        Debug.Log(student.GetId() + " starts the day");
                    
                        Vector3 spawnPoint = UnityEngine.Random.insideUnitSphere * 30f + student.GetSpawnPoint();

                        NavMeshHit hit;

                        NavMesh.SamplePosition (spawnPoint, out hit, 30f, 1);

                        Vector3 pointOnNavMesh = hit.position;

                        foreach (GameObject door in _doors) {
                            if (int.Parse(door.tag) == student.GetCurrentLecture().building) {
                                student.SetDoorsWithinCurrentComplex(door);
                            } 
                        }

                        instantiatedAgent = (GameObject)Instantiate(agent, pointOnNavMesh, transform.rotation);
                        instantiatedAgent.GetComponent<NavMeshAgentController>().Activate(FindClosestDoor(student)[1], student);
                        student.SetCurrentlyEnRoute(true);
                        student.SetNextLecture();

                    } else if ((student.GetLectureIndex() >= student.lectureList.Count) 
                            && (student.GetCurrentLecture().GetEndInMinutes() <= gameTime)) { // All lectures finished

                        Debug.Log(student.GetId() + " finished his lectures"); 
                        instantiatedAgent = (GameObject)Instantiate(agent, FindClosestDoor(student)[0], transform.rotation);
                        instantiatedAgent.GetComponent<NavMeshAgentController>().Activate(FindClosestDoor(student)[1], student);
                        student.SetCurrentlyEnRoute(true);
                        student.SetNextLecture();
                        student.SetDayFinished();
                        studentsFinished++;

                    } else if (gameTime >= nextLectureBegin - 15) { // Spawn agent commute to next lecture
                        student.EmptyCurrentLectureDoorList();
                        Debug.Log(student.GetId() + " finished his first Lecture");
                        foreach (GameObject door in _doors) {
                            if (int.Parse(door.tag) == student.GetCurrentLecture().building) {
                                student.SetDoorsWithinCurrentComplex(door);
                            }
                        } 

                        instantiatedAgent = (GameObject)Instantiate(agent, FindClosestDoor(student)[0], transform.rotation);
                        instantiatedAgent.GetComponent<NavMeshAgentController>().Activate(FindClosestDoor(student)[1], student); 
                        student.SetCurrentlyEnRoute(true);
                        student.SetNextLecture();

                    } else if ((student.GetCurrentLecture().building == student.GetNextLecture().building) 
                            && (student.GetLectureIndex() > 0)) { // Next lecture is in the same complex
                        student.SetNextLecture();
                        Debug.Log(student.GetId() + "´s next lecture is in the same complex");
                    } else {
                        // Debug.Log("Event not accounted for");
                        continue;
                    }
                } else
                throw new System.Exception("hab ich dich du Schlingel");
            }
            Debug.Log("Students finished: " + studentsFinished);
            yield return new WaitForSeconds(7.5f);
        }
    }
    */

    private IEnumerator Spawn2() {
        _studentList = CommuteController.CommuteControllerInstance.GetStudentList();
        int studentListSize = _studentList.Length;

        while (true) {
            int studentsFinished = 0;

            int gameTime = (int) ClockManagement.ClockManagementInstance.GetTime();
            GameObject instantiatedAgent = null;

            foreach (Student student in _studentList) {
                if(student.check(gameTime) == 1) {
                    string[] routePoints = student.RoutePoints();

                    Vector3 spawnPoint = UnityEngine.Random.insideUnitSphere * 30f + FindDoor(routePoints[0]).transform.position;
                    NavMeshHit hit;
                    NavMesh.SamplePosition (spawnPoint, out hit, 30f, 1);
                    spawnPoint = hit.position;

                    instantiatedAgent = (GameObject)Instantiate(agent, spawnPoint, transform.rotation);
                    //instantiatedAgent.GetComponent<NavMeshAgentController>().Activate(FindDoor(routePoints[1]), spawnPoint);
                    NavMeshPath path = _pathList[NameToIndex(FindDoor(routePoints[0]).name), NameToIndex(FindDoor(routePoints[1]).name)];
                    
                    instantiatedAgent.GetComponent<NavMeshAgent>().SetPath(path);
    
                } else if(student.check(gameTime) == 2) {
                    studentsFinished++;
                }
            }
            if(studentsFinished > 100) {
                break;
            }
            _cM.SetGo();
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
        foreach(GameObject startDoor in _doors) {
            foreach(GameObject finishDoor in _doors) {
                if(startDoor.tag == finishDoor.tag) { //doors at same house
                    continue;
                }
                agent.transform.position = startDoor.transform.position;
                NavMeshPath path = new NavMeshPath();
                agent.GetComponent<NavMeshAgent>().CalculatePath(finishDoor.transform.position, path);
                //remove first corner of the path
                if(path.corners.Length > 1) {
                    agent.transform.position = path.corners[1];
                    agent.GetComponent<NavMeshAgent>().CalculatePath(finishDoor.transform.position, path);
                }

                if(path == null) {
                    Debug.Log(startDoor.name + "  " + finishDoor.name);
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

}
