using UnityEngine;

class StudentInitialisation : MonoBehaviour {

    private int _numberOfStudents = 10000;
    private static StudentInitialisation _studentInitialisationInstance;

    [SerializeField] private Student[] _studentList;


    // Instance for reference during run time
    private void Awake() {
        _studentInitialisationInstance = this;
    }


    private void Start() {
        Initialize();
    }


    public static StudentInitialisation StudentInitialisationInstance {
        get {return _studentInitialisationInstance;}
    }


    public Student[] GetStudentList() {
        return _studentList;
    }


    private void Initialize() {

        Debug.Log("Initialize");

        _studentList = new Student[_numberOfStudents];

        int medStudents = Mathf.RoundToInt((float)_numberOfStudents * 0.28f);
        int mathStudents = Mathf.RoundToInt((float)_numberOfStudents * 0.13f);
        int chemStudents = Mathf.RoundToInt((float)_numberOfStudents * 0.15f);
        int phyStudents = Mathf.RoundToInt((float)_numberOfStudents * 0.19f);
        int bioStudents = Mathf.RoundToInt((float)_numberOfStudents * 0.19f);
        int clsStudents = Mathf.RoundToInt((float)_numberOfStudents * 0.06f);

        for (int i = 0; i < _numberOfStudents; i++) {

            float size = Random.Range(1.5f, 2.2f);
            float speed = Random.Range(0.8f, 1.2f);

            Student student = new Student(i, size, speed);

            // Assign each student a faculty
            if (medStudents > 0) {
                student.SetFaculty(0);
                medStudents--;
            } else if(mathStudents > 0) {
                student.SetFaculty(1);
                mathStudents--;
            } else if (chemStudents > 0) {
                student.SetFaculty(2);
                chemStudents--;
            } else if (phyStudents > 0) {
                student.SetFaculty(3);
                phyStudents--;
            } else if (bioStudents > 0) {
                student.SetFaculty(4);
                bioStudents--;
            } else {
                student.SetFaculty(6);
                clsStudents--;
            }

            _studentList[i] = student;
        }
        GameManager.GameManagerInstance.UpdateGameState(GameState.SetAgentCommute);
    }
}