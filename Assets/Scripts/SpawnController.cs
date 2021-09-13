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
            //StartCoroutine(Spawn());
            test();
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

        Debug.Log(_doors[1].tag);
    }

    void test () {
        _studentList = CommuteController.CommuteControllerInstance.getStudentList();
            int number = 0;
            int[] facultydistribution = new int[7];

            foreach (Student student in _studentList) {
                if (student.lectureList.Count > 0) {
                    number++;
                }

                if(student.lectureList.Count == 0) {
                    facultydistribution[student.getFaculty()]++;
                }

                
            }
            for(int i=0; i<7; i++) {
                Debug.Log(facultydistribution[i]);
            }
            Debug.Log("number of students wis a forlesung: " +number);
            Debug.Log("students insgesamt: " + _studentList.Length);
    }

/*
    IEnumerator Spawn () {
        while (true) {
            _studentList = CommuteController.CommuteControllerInstance.getStudentList();
            int number = 0;

            foreach (Student student in _studentList) {
                if (student.lectureList.Count > 0) {
                    int time = student.getNextLecture();
                        // TODO if gametime - 15 == time
                        Instantiate(_Agent, student.getSpawnPoint(), transform.rotation);


                    Debug.Log(student.lectureList[0].building);
                    number++;
                }
            }
            Debug.Log(number);
            yield return new WaitForSeconds(7.5f);
        }
    }

    
    /*
    void Spawn() {

        Debug.Log("SpawnAgents");
        
        for (int i = 1; i < agentCount; i++) {

            Vector3 randomPoint = UnityEngine.Random.insideUnitSphere * range;
            NavMeshHit hit;

            NavMesh.SamplePosition (randomPoint,
                                    out hit,
                                    range,
                                    1);

            Vector3 point = hit.position;
            
            Instantiate(_Agent,
                        point,
                        transform.rotation);
        }

        GameManager.GameManagerInstance.UpdateGameState(GameState.SetAgentCommute);
    }
    */
}
