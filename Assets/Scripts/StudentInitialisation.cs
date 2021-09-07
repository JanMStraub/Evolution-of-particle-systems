using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class StudentInitialisation : MonoBehaviour {

    public float studentInitialisationProgress;

    public bool isDone;

    public int numberOfStudents = 10322;

    private static StudentInitialisation _StudentInitialisationInstance;

    [SerializeField] Student[] _studentList;

    void Awake() {
        GameManager.OnGameStateChanced += GameManagerOnGameStateChanged;
        _StudentInitialisationInstance = this;
    }

    void OnDestroy() {
        GameManager.OnGameStateChanced -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged (GameState state) {
         if (state == GameState.StudentInitialisation) {
            Initialize ();
        }
    }

    public static StudentInitialisation StudentInitialisationInstance {
        get {return _StudentInitialisationInstance;}
    }

    public Student[] getStudentList () {
        return _studentList;
    }

    void Initialize () {

        _studentList = new Student[numberOfStudents];

        int medstudents = Mathf.RoundToInt((float)numberOfStudents * 0.28f);
        int mathstudents = Mathf.RoundToInt((float)numberOfStudents * 0.13f);
        int chemstudents = Mathf.RoundToInt((float)numberOfStudents * 0.15f);
        int phystudents = Mathf.RoundToInt((float)numberOfStudents * 0.19f);
        int biostudents = Mathf.RoundToInt((float)numberOfStudents * 0.3f);
        int clstudents = Mathf.RoundToInt((float)numberOfStudents * 0.3f);

        for (int i = 0; i < numberOfStudents; i++) {

            float size = Random.Range(1.5f, 2.2f);
            float speed = Random.Range(45f, 60f);

            Student student = new Student(i, size, speed);

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

        GameManager.GameManagerInstance.UpdateGameState(GameState.RunSimulation);
    }
}