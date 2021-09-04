using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CommuteController : MonoBehaviour {    

    private GameObject _GameManager;

    private LectureList _lectureList;

    [SerializeField] GameObject[] _homeList;

    [SerializeField] GameObject[] _workList;

    [SerializeField] GameObject[] _agentList;

    void Awake() {
        GameManager.OnGameStateChanced += GameManagerOnGameStateChanged;
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
            Assign();
            aLtS(); //addLecturestoStudent 2.0
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

        foreach (GameObject agent in _agentList) {

            int medstudents = 2905;
            int mathstudents = 1340;
            int chemstudents = 1509;
            int phystudents = 1975;
            int biostudents = 1989;
            int clstudents = 300;
            
            int homeNumber = Random.Range(0, _homeList.Length);
            agent.GetComponent<NavMeshAgentController>()
                 .setHomeDestination(_homeList[homeNumber]);
            
            // Assign each agent a student
            Student student = new Student(agent.GetInstanceID());

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
                        
            
            agent.GetComponent<NavMeshAgentController>()._student = student;
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

        GameManager.GameManagerInstance.UpdateGameState(GameState.StartNavMeshAgents);
    }

    /*
    void addLecturesToStudents () {

        foreach (var lecture in _lectureList.lecture) {
            
            if (lecture.faculty == (int)FacultyIndexes.Medizin) {
                student.lectureList.Add(lecture);
                Debug.Log(student.lectureList[0].building);
            } else if (lecture.faculty == (int)FacultyIndexes.Mathematik_und_Informatik) {
                
            } else if (lecture.faculty == (int)FacultyIndexes.Chemie_und_Geowissenschaften) {
                
            } else if (lecture.faculty == (int)FacultyIndexes.Physik_und_Astronomie) {
                
            } else if (lecture.faculty == (int)FacultyIndexes.Biowissenschaften) {
                
            } else if (lecture.faculty == (int)FacultyIndexes.Rest) {
                
            } else if (lecture.faculty == (int)FacultyIndexes.Computerlinguistik) {
                
            } else {
                Debug.Log("Lecture type not found!");
            }
        }
    }
    */

    void aLtS()
    {
        int studentIndex = 0;
        int[] freeSlots = new int[7]{3394, 1566, 1763, 2307, 2324, 355, 351};
        while(_lectureList.Size() > 0)
        {
            Student student = _agentList[studentIndex].GetComponent<NavMeshAgentController>()._student;
            bool searchOwn = (freeSlots[(int)student.getFaculty()])>0? true : false;
            Lecture lecture = FindLecture(searchOwn, student);
            if(lecture != null)
            {
                student.lectureList.Add(lecture);
                student.setTimetableEnd(lecture.GetEndInMinutes());
                lecture.number--;
                if(lecture.number == 0)
                {
                    _lectureList.lecture.Remove(lecture);
                }
                freeSlots[lecture.faculty]--;
            }


            studentIndex = (studentIndex+1)%_agentList.Length;
        }
    }

    Lecture FindLecture(bool searchOwn, Student student)
    {
        int tryCounter = 0;
        bool fit = false;
        Lecture lecture = null;
        while(!fit)
        {
            lecture =_lectureList.lecture[(int)Random.Range(0,_lectureList.Size())];
            if(lecture.faculty == student.getFaculty() || !searchOwn)
            {
                fit = true;
            }
            if(student.getTimetableEnd() > lecture.GetStartInMinutes())
            {
                fit = false;
            }
            if(tryCounter > 50)
            {
                break;
            }
            tryCounter++;
        }
        return lecture;
    }
}