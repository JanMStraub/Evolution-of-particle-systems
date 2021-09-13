using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class StudentInitialisation : MonoBehaviour {

    public float studentInitialisationProgress;

    public bool isDone;

    private int _numberOfStudents = 10000;

    private static StudentInitialisation _StudentInitialisationInstance;
    
    private GameObject[] _spawnPoints;

    [SerializeField] private Student[] _studentList;

    void Awake() {
        _StudentInitialisationInstance = this;
    }

    void Start () {
        _spawnPoints = GameObject.FindGameObjectsWithTag("Spawn");
        Initialize ();
    }

    public static StudentInitialisation StudentInitialisationInstance {
        get {return _StudentInitialisationInstance;}
    }
    // test
    public Student[] getStudentList () {
        return _studentList;
    }

    void Initialize () {

        Debug.Log("Initialize");

        _studentList = new Student[_numberOfStudents];

        int medstudents = Mathf.RoundToInt((float)_numberOfStudents * 0.28f);
        int mathstudents = Mathf.RoundToInt((float)_numberOfStudents * 0.13f);
        int chemstudents = Mathf.RoundToInt((float)_numberOfStudents * 0.15f);
        int phystudents = Mathf.RoundToInt((float)_numberOfStudents * 0.19f);
        int biostudents = Mathf.RoundToInt((float)_numberOfStudents * 0.19f);
        int clstudents = Mathf.RoundToInt((float)_numberOfStudents * 0.06f);

        for (int i = 0; i < _numberOfStudents; i++) {

            float size = Random.Range(1.5f, 2.2f);
            float speed = Random.Range(45f, 60f);
            int spawnPoint = Random.Range(0, _spawnPoints.Length);

            Student student = new Student(i, size, speed, _spawnPoints[spawnPoint].transform.position);

            // Assign each student a faculty
            if (medstudents > 0) {
                student.setFaculty(0);
                medstudents--;
            } else if(mathstudents > 0) {
                student.setFaculty(1);
                mathstudents--;
            } else if (chemstudents > 0) {
                student.setFaculty(2);
                chemstudents--;
            } else if (phystudents > 0) {
                student.setFaculty(3);
                phystudents--;
            } else if (biostudents > 0) {
                student.setFaculty(4);
                biostudents--;
            } else {
                student.setFaculty(6);
                clstudents--;
            }

            _studentList[i] = student;
        }

        GameManager.GameManagerInstance.UpdateGameState(GameState.SetAgentCommute);
    }
}