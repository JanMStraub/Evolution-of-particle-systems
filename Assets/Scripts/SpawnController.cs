using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

class SpawnController : MonoBehaviour {

    // TODO refactor

    private static SpawnController _SpawnControllerInstance;

    private Student[] _studentList;

    [SerializeField] List<GameObject> _doors = new List<GameObject>();
    
    public GameObject _Agent;
    
    public static SpawnController SpawnControllerInstance {
        get {return _SpawnControllerInstance;}
    }

    void Awake() {
        GameManager.OnGameStateChanced += GameManagerOnGameStateChanged;
        _SpawnControllerInstance = this;
    }


    void OnDestroy() {
        GameManager.OnGameStateChanced -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged (GameState state) {
         if (state == GameState.RunSimulation) {
            _studentList = StudentInitialisation.StudentInitialisationInstance.getStudentList();
            StartCoroutine(Spawn());
        }
    }

    void Start () {

        /*

        234/235/236 zusammen als 234
        306/308/324/325/327/328/329/345/346/348 zusammen als 306
        304 Mensa
        700/720 zusammen als 700


        Mensagebäude noch einfügen und Mittagspause reinbringen !ACHTUNG: Teilweise Vorlesungen den ganzen Tag -> Ausnahmen?

        */

        GameObject doors_parent = GameObject.FindGameObjectWithTag("ComplexController");

        foreach (Transform door in doors_parent.transform) {
            _doors.Add(door.gameObject);
        }
    }

    IEnumerator Spawn () {
        while (true) {
            _studentList = CommuteController.CommuteControllerInstance.getStudentList();
            int studentsFinished = 0;
            GameObject instantiated = null;

            foreach (Student student in _studentList) {
                if (student.lectureList.Count > 0) {
                    int nextLectureBegin = student.getNextLecture().GetStartInMinutes();
                    int gameTime = (int) ClockManagement.ClockManagementInstance.GetTime();

                    if (student.getLectureIndex() == 0) { // First lecture of the day
                        instantiated = (GameObject)GameObject.Instantiate(_Agent, student.getSpawnPoint(), transform.rotation);
                        instantiated.GetComponent<NavMeshAgent>().SetDestination(FindClosestDoor(student)[1].transform.position);

                        foreach (GameObject door in _doors) {
                            if (int.Parse(door.tag) == student.getCurrentLecture().building) {
                                student.setDoorsWithinCurrentComplex(door);
                            }
                        }
                    } else if (student.getLectureIndex() > student.lectureList.Count) { // All lectures finished
                        Debug.Log(student.getId() + " finished his lectures"); 
                        studentsFinished++;
                    } else if (student.getCurrentLecture().building == student.getNextLecture().building) { // Next lecture is in the same complex
                        break;
                    } else {
                        if (gameTime <= nextLectureBegin - 15 && gameTime + 15 >= nextLectureBegin) // Spawn agent commute to next lecture
                            instantiated = (GameObject)GameObject.Instantiate(_Agent, FindClosestDoor(student)[0].transform.position, transform.rotation);
                            instantiated.GetComponent<NavMeshAgent>().SetDestination(FindClosestDoor(student)[1].transform.position);
                    }
                }
            }
            Debug.Log("Students finished: " + studentsFinished);
            yield return new WaitForSeconds(7.5f);
        }
    }

    GameObject[] FindClosestDoor (Student student) {
        
        List<GameObject> doorsWithinNextComplex = new List<GameObject>();
        GameObject[] closestDoors = new GameObject[2];

        foreach (GameObject door in _doors) {
            if (int.Parse(door.tag) == student.getNextLecture().building) {
                doorsWithinNextComplex.Add(door);
            }
        }

        if (doorsWithinNextComplex.Count != 0 && student.getDoorsWithinCurrentComplex().Count != 0) {
            float Distance = 100000000;
            foreach (GameObject currentDoor in student.getDoorsWithinCurrentComplex()) {
                foreach (GameObject nextDoor in doorsWithinNextComplex) {
                    float currentDistance = Vector3.Distance(currentDoor.transform.position, nextDoor.transform.position);
                    if (Distance > currentDistance) {
                        closestDoors[0] = currentDoor;
                        closestDoors[1] = nextDoor;
                    }
                }
            }
        }

        return closestDoors;
    }
}
