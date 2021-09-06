using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CommuteController : MonoBehaviour {    
    
    public float commuteProgress;

    public bool isDone;

    private static CommuteController _CommuteControllerInstance;

    private int _progress = 1;

    private GameObject _GameManager;

    private LectureList _lectureList;

    [SerializeField] GameObject[] _homeList;

    [SerializeField] GameObject[] _workList;

    [SerializeField] GameObject[] _agentList;

    [SerializeField] Student[] _studentList;

    public static CommuteController CommuteControllerInstance {
        get {return _CommuteControllerInstance;}
    }

    void Awake() {
        GameManager.OnGameStateChanced += GameManagerOnGameStateChanged;
        _CommuteControllerInstance = this;
    }

    void Start () {
        _GameManager = GameObject.FindGameObjectWithTag("GameController");
        JSONReader jsonreader = _GameManager.GetComponent<JSONReader>();
        _lectureList = jsonreader.myLectureList;
        _studentList = new Student[10322];
    }

    void OnDestroy() {
        GameManager.OnGameStateChanced -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged (GameState state) {
         if (state == GameState.SetAgentCommute) {
            Assign();
            addLecturesToStudents();
        }
    }

    void Assign() {      

        // Get list of agents
        if (_agentList.Length == 0)
            _agentList = GameObject.FindGameObjectsWithTag("Agent");
            // Debug.Log("Agent list created");

        // Assign home to agent
        if (_homeList.Length == 0)
            _homeList = GameObject.FindGameObjectsWithTag("HomeDoor");
            // Debug.Log("Home list created");

        foreach(GameObject agent in _agentList)
        {
            int homeNumber = Random.Range(0, _homeList.Length);
            agent.GetComponent<NavMeshAgentController>()
                 .setHomeDestination(_homeList[homeNumber]);
        }

        int assistIndex = 0;

        int medstudents = 2905;
        int mathstudents = 1340;
        int chemstudents = 1509;
        int phystudents = 1975;
        int biostudents = 1989;
        int clstudents = 300;
            

        for(int i=0; i<_studentList.Length; i++) {
            
            // Assign each agent a student
            Student student = new Student(i);

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

            assistIndex++;
        }

        // Assign work to agent
        if (_workList.Length == 0)
            _workList = GameObject.FindGameObjectsWithTag("WorkDoor");
            // Debug.Log("Work list created");

        foreach (GameObject agent in _agentList) {
            int workNumber = Random.Range(0, _workList.Length);
            agent.GetComponent<NavMeshAgentController>()
                 .setWorkDestination(_workList[workNumber]);
            // Debug.Log("Work assigned to " + agent.GetInstanceID());
        } 

        commuteProgress = ((int)_progress / SpawnController.SpawnControllerInstance.agentCount);

        _progress++;

        isDone = true;

        GameManager.GameManagerInstance.UpdateGameState(GameState.StartNavMeshAgents);
    }

    void addLecturesToStudents() {
        int studentIndex = 0;
        int[] freeSlots = new int[7]{3394, 1566, 1763, 2307, 2324, 355, 351};
        while (_lectureList.Size() > 50) {
            //Student student = _agentList[studentIndex].GetComponent<NavMeshAgentController>()._student;
            Student student = _studentList[studentIndex];
            bool searchOwn = (freeSlots[(int)student.getFaculty()])>0? true : false;
            Lecture lecture = FindLecture(searchOwn, student);
            if (lecture != null) {
                student.lectureList.Add(lecture);
                student.setTimetableEnd(lecture.GetEndInMinutes());
                lecture.number--;
                if (lecture.number == 0) {
                    _lectureList.lecture.Remove(lecture);
                }
                freeSlots[lecture.faculty]--;
            }


            studentIndex = (studentIndex + 1) % _agentList.Length;
            Debug.Log(_lectureList.Size());
        }
    }

    Lecture FindLecture(bool searchOwn, Student student) {
        Lecture lecture;
        for (int i = 0; i < 100; i++) {

            lecture =_lectureList.lecture[(int)Random.Range(0,_lectureList.Size())];
            
            if ((lecture.faculty == student.getFaculty() || !searchOwn) && 
                (student.getTimetableEnd() > lecture.GetStartInMinutes())) {
                    return lecture;
            } 
        }
        return null;
    }
}