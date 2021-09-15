using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

class SpawnController : MonoBehaviour {

    private static SpawnController _spawnControllerInstance;
    private Student[] _studentList;

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
            }
            ClockManagement.ClockManagementInstance.StartTime();
            StartCoroutine(Spawn());
        }
    }
        /*

        234/235/236 zusammen als 234
        306/308/324/325/327/328/329/345/346/348 zusammen als 306
        304 Mensa
        700/720 zusammen als 700


        Mensagebäude noch einfügen und Mittagspause reinbringen !ACHTUNG: Teilweise Vorlesungen den ganzen Tag -> Ausnahmen?

        */


    IEnumerator Spawn() {
        int studentsFinished = 0;
        int studentListSize = _studentList.Length;

        while (studentsFinished < studentListSize) {
            _studentList = CommuteController.CommuteControllerInstance.GetStudentList();
            GameObject instantiatedAgent = null;

            foreach (Student student in _studentList) {
                if (student.lectureList.Count > 0) {
                    int nextLectureBegin = student.GetCurrentLecture().GetStartInMinutes();
                    int gameTime = (int) ClockManagement.ClockManagementInstance.GetTime();

                    if ((student.GetLectureIndex() == 0) && (gameTime >= nextLectureBegin - 15)) { // First lecture of the day
                    
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
                        instantiatedAgent.GetComponent<NavMeshAgentController>().Activate(FindClosestDoor(student)[1]);
                        student.SetNextLecture();

                    } else if ((student.GetLectureIndex() >= student.lectureList.Count) && (student.GetCurrentLecture().GetEndInMinutes() <= gameTime)) { // All lectures finished
                        Debug.Log(student.GetId() + " finished his lectures"); 
                        instantiatedAgent = (GameObject)Instantiate(agent, FindClosestDoor(student)[0], transform.rotation);
                        instantiatedAgent.GetComponent<NavMeshAgentController>().Activate(FindClosestDoor(student)[1]);
                        student.SetNextLecture();
                        studentsFinished++;
                    } else if (student.GetCurrentLecture().building == student.GetNextLecture().building) { // Next lecture is in the same complex
                        continue;
                    } else {
                        if (gameTime >= nextLectureBegin - 15) {// Spawn agent commute to next lecture
                            student.EmptyCurrentLectureDoorList();

                            foreach (GameObject door in _doors) {
                                if (int.Parse(door.tag) == student.GetCurrentLecture().building) {
                                    student.SetDoorsWithinCurrentComplex(door);
                                }
                            } 

                            instantiatedAgent = (GameObject)Instantiate(agent, FindClosestDoor(student)[0], transform.rotation);
                            instantiatedAgent.GetComponent<NavMeshAgentController>().Activate(FindClosestDoor(student)[1]); 
                            student.SetNextLecture();
                        }
                    }
                }
            }
            Debug.Log("Students finished: " + studentsFinished);
            yield return new WaitForSeconds(7.5f);
        }
    }


    private Vector3[] FindClosestDoor(Student student) {
        
        List<GameObject> doorsWithinNextComplex = new List<GameObject>();
        Vector3[] closestDoors = new Vector3[2];
        float distance = 100000000;

        foreach (GameObject door in _doors) {
            if (int.Parse(door.tag) == student.GetNextLecture().building) {
                doorsWithinNextComplex.Add(door);
            }
        }

        if (doorsWithinNextComplex.Count != 0 && student.GetDoorsWithinCurrentComplex().Count != 0) {
            foreach (GameObject currentDoor in student.GetDoorsWithinCurrentComplex()) {
                foreach (GameObject nextDoor in doorsWithinNextComplex) {
                    float currentDistance = Vector3.Distance(currentDoor.transform.position, nextDoor.transform.position);
                    if (distance > currentDistance) {
                        closestDoors[0] = currentDoor.transform.position;
                        closestDoors[1] = nextDoor.transform.position;
                    }
                }
            }
        } else if (student.GetLectureIndex() == 0) { //early in the morning
            closestDoors[0] = student.GetSpawnPoint();

            foreach (GameObject nextDoor in doorsWithinNextComplex) {
                    float currentDistance = Vector3.Distance(student.GetSpawnPoint(), nextDoor.transform.position);
                    if (distance > currentDistance) {
                        closestDoors[1] = nextDoor.transform.position;
                    }
            }
        } else if (student.GetLectureIndex() > student.lectureList.Count) { //day over, going home
            closestDoors[1] = student.GetSpawnPoint();

            foreach (GameObject currentDoor in student.GetDoorsWithinCurrentComplex()) {
                    float currentDistance = Vector3.Distance(student.GetSpawnPoint(), currentDoor.transform.position);
                    if (distance > currentDistance) {
                        closestDoors[0] = currentDoor.transform.position;
                    }
            }
        }  else {
            throw new System.Exception("illegal position");
        }
        return closestDoors;
    }
}
