using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CommuteController : MonoBehaviour {    

    private GameObject _GameManager;

    private LectureList _lectureList;

    [SerializeField] GameObject[] _homeList;

    [SerializeField] GameObject[] _workList;

    [SerializeField] GameObject[] _agentList;

    /*
    //Medizin, MatheInfo, ChemieGeo, PhsyikAstro, Bio, ComputerLinguistik, Rest
    JSONReader.LectureList medLectures = new JSONReader.LectureList(); // Medizin = 0
    JSONReader.LectureList matLectures = new JSONReader.LectureList(); // Mathematik_und_Informatik = 1
    JSONReader.LectureList cheLectures = new JSONReader.LectureList(); // Chemie_und_Geowissenschaften = 2
    JSONReader.LectureList phyLectures = new JSONReader.LectureList(); // Physik_und_Astronomie = 3
    JSONReader.LectureList bioLectures = new JSONReader.LectureList(); // Biowissenschaften = 4
    JSONReader.LectureList restLectures = new JSONReader.LectureList(); // Rest = 5
    JSONReader.LectureList comLectures = new JSONReader.LectureList(); // Computerlinguistik = 6
    */

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
        }
    }

    void Assign() {      

        getFacultyFromJSON();

        // Get list of agents
        if (_agentList.Length == 0)
            _agentList = GameObject.FindGameObjectsWithTag("Agent");
            // Debug.Log("Agent list created");

        // Assign home to agent
        if (_homeList.Length == 0)
            _homeList = GameObject.FindGameObjectsWithTag("HomeDoor");
            // Debug.Log("Home list created");

        foreach (GameObject agent in _agentList) {
            /*
            int homeNumber = Random.Range(0, _homeList.Length);
            agent.GetComponent<NavMeshAgentController>()
                 .setHomeDestination(_homeList[homeNumber]);
            */
            // Debug.Log("Home assigned to " + agent.GetInstanceID());
            
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

    void getFacultyFromJSON () {

        foreach (var lecture in _lectureList.lecture) {
            
            if (lecture.faculty == (int)FacultyIndexes.Medizin) {
                Student student = new Student(1, 0);
                student.addLecture(lecture);
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

        /*
        foreach (var lecture in _lectureList.lecture) {

            if (lecture.faculty == (int)FacultyIndexes.Medizin) {
                JSONReader.Lecture lec = new JSONReader.Lecture(lecture.faculty,
                                          lecture.building,
                                          lecture.number,
                                          lecture.start,
                                          lecture.end);
                medLectures.add(lec);
            } else if (lecture.faculty == (int)FacultyIndexes.Mathematik_und_Informatik) {
                JSONReader.Lecture lec = new JSONReader.Lecture(lecture.faculty,
                                          lecture.building,
                                          lecture.number,
                                          lecture.start,
                                          lecture.end);
                matLectures.add(lec);
            } else if (lecture.faculty == (int)FacultyIndexes.Chemie_und_Geowissenschaften) {
                JSONReader.Lecture lec = new JSONReader.Lecture(lecture.faculty,
                                          lecture.building,
                                          lecture.number,
                                          lecture.start,
                                          lecture.end);
                cheLectures.add(lec);
            } else if (lecture.faculty == (int)FacultyIndexes.Physik_und_Astronomie) {
                JSONReader.Lecture lec = new JSONReader.Lecture(lecture.faculty,
                                          lecture.building,
                                          lecture.number,
                                          lecture.start,
                                          lecture.end);
                phyLectures.add(lec);
            } else if (lecture.faculty == (int)FacultyIndexes.Biowissenschaften) {
                JSONReader.Lecture lec = new JSONReader.Lecture(lecture.faculty,
                                          lecture.building,
                                          lecture.number,
                                          lecture.start,
                                          lecture.end);
                bioLectures.add(lec);
            } else if (lecture.faculty == (int)FacultyIndexes.Rest) {
                JSONReader.Lecture lec = new JSONReader.Lecture(lecture.faculty,
                                          lecture.building,
                                          lecture.number,
                                          lecture.start,
                                          lecture.end);
                restLectures.add(lec);
            } else if (lecture.faculty == (int)FacultyIndexes.Computerlinguistik) {
                JSONReader.Lecture lec = new JSONReader.Lecture(lecture.faculty,
                                          lecture.building,
                                          lecture.number,
                                          lecture.start,
                                          lecture.end);
                comLectures.add(lec);
            } else {
                Debug.Log("Lecture type not found!");
            }
        }
        */
        /*
        Debug.Log(medLectures.lecture.Capacity);
        Debug.Log(matLectures.lecture.Capacity);
        Debug.Log(cheLectures.lecture.Capacity);
        Debug.Log(phyLectures.lecture.Capacity);
        Debug.Log(bioLectures.lecture.Capacity);
        Debug.Log(comLectures.lecture.Capacity);
        Debug.Log(restLectures.lecture.Capacity);
        */
    }
}