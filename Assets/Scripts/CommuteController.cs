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
    }

    void OnDestroy() {
        GameManager.OnGameStateChanced -= GameManagerOnGameStateChanged;
    }

    private void GameManagerOnGameStateChanged (GameState state) {
         if (state == GameState.SetAgentCommute) {
            Debug.Log("commute test");
            _studentList = StudentInitialisation.StudentInitialisationInstance.getStudentList();
            Debug.Log(_studentList.GetLength(1));
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

        GameManager.GameManagerInstance.UpdateGameState(GameState.RunSimulation);
    }

    void addLecturesToStudents(){
        int lectureListSize = _lectureList.Size(); //better performance
        int studentIndex = 0;
        int[] freeSlots = new int[7]{1023, 844, 408, 3528, 611, 10282, 42};
        while(lectureListSize > 25) {
            //Student student = _agentList[studentIndex].GetComponent<NavMeshAgentController>()._student;
            Student student = _studentList[studentIndex];
            bool searchOwn = (freeSlots[(int)student.getFaculty()])>0? true : false;
            Lecture lecture = FindLecture(searchOwn, student);
            if (lecture != null) {
                Debug.Log(studentIndex);
                student.lectureList.Add(lecture);
                student.setTimetableEnd(lecture.GetEndInMinutes());
                lecture.number--;
                if (lecture.number == 0) {
                    _lectureList.lecture.Remove(lecture);
                    lectureListSize--;
                }
                freeSlots[lecture.faculty]--;
            }

            studentIndex = (studentIndex + 1) % _studentList.Length;
            //Debug.Log(_lectureList.Size());
        }
    }

    Lecture FindLecture(bool searchOwn, Student student) {
        Lecture lecture;
        for (int i = 0; i < 100; i++) { //100 tries max to find a fitting lecture

            lecture =_lectureList.lecture[(int)Random.Range(0,_lectureList.Size())];
            
            if (lecture.faculty == student.getFaculty() || !searchOwn) {
                if (student.getTimetableEnd() < lecture.GetStartInMinutes())
                {
                    //Debug.Log("Vorlesung zugewiesen");
                    return lecture;
                    
                }
            } 
        }
        return null;
    }
}