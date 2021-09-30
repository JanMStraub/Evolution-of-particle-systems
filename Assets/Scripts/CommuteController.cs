using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CommuteController : MonoBehaviour {    
    
    // public float commuteProgress;
    // public bool isDone;

    private GameObject _gameManager;
    private LectureList _lectureList;
    private Student[] _studentList;
    private static CommuteController _commuteControllerInstance;
    // private int _progress = 1;

    [SerializeField] GameObject[] _agentList;


    public static CommuteController CommuteControllerInstance {
        get {return _commuteControllerInstance;}
    }


    private void Awake() {
        GameManager.onGameStateChanced += GameManagerOnGameStateChanged;
        _commuteControllerInstance = this;
    }


    private void Start() {
        _gameManager = GameObject.FindGameObjectWithTag("GameController");
        JSONReader jsonReader = _gameManager.GetComponent<JSONReader>();
        _lectureList = jsonReader.myLectureList;
    }


    private void OnDestroy() {
        GameManager.onGameStateChanced -= GameManagerOnGameStateChanged;
    }


    private void GameManagerOnGameStateChanged(GameState state) {
         if (state == GameState.SetAgentCommute) {
            _studentList = StudentInitialisation.StudentInitialisationInstance.GetStudentList();
            
            AddLecturesToStudents();
            AddLunch();
            GameManager.GameManagerInstance.UpdateGameState(GameState.RunSimulation);
        }
    }


    public Student[] GetStudentList() {
        return _studentList;
    }


    private void AddLecturesToStudents(){
        int studentIndex = 0;

        int[] freeSlots = new int[7];
        foreach (Lecture lecture in _lectureList.lecture) { //count all available seats in all lectures, sorted to faculties
            freeSlots[lecture.faculty] += lecture.number;
        }

        int controllSlotNumber = 16736; //ist zwar falsch aber muss so
        int actualSlotNumber = 16735;
        bool nothingChanged = false;

        while(!nothingChanged) {
            //Student student = _agentList[studentIndex].GetComponent<NavMeshAgentController>()._student;
            Student student = _studentList[studentIndex];
            bool searchOwn = false;
            if (freeSlots[(int)student.GetFaculty()] > 0) {
                searchOwn = true;
            }

            Lecture lecture = FindLecture(searchOwn, student);

            if (lecture != null) {
                student.lectureList.Add(lecture);
                student.SetTimetableEnd(lecture.GetEndInMinutes());
                lecture.number--;

                if (lecture.number == 0) {
                    _lectureList.lecture.Remove(lecture);
                }

                freeSlots[lecture.faculty]--;
                actualSlotNumber--;
            } 

            if(studentIndex == 0) { 
                if(controllSlotNumber == actualSlotNumber) {
                    nothingChanged = true; // break if all students cant get more lessons;
                }
                controllSlotNumber = actualSlotNumber;
            }
            studentIndex = (studentIndex + 1) % _studentList.Length;
        }
        Debug.Log("free slots left: " + actualSlotNumber);
    }


    private Lecture FindLecture(bool searchOwn, Student student) {
        for (int i=0; i<100; i++) { //100 tries max to find a fitting lecture

            Lecture lecture =_lectureList.lecture[Random.Range(0,_lectureList.Size())];
            
            if ((lecture.faculty == student.GetFaculty()) || !searchOwn) {
                if (student.GetTimetableEnd() < lecture.GetStartInMinutes())
                {
                    return lecture;
                }
            } 
        }
        return null;
    }
    
    private void AddLunch() {

        foreach(Student student in _studentList) {
            Lecture lunch = new Lecture();
            lunch.faculty = 42;
            lunch.number = 10001;
            lunch.start = "11:00"; //660
            lunch.end = "14:00"; //840

            bool noLunch = false;

            foreach (Lecture lecture in student.lectureList) {
                if(lecture.GetStartInMinutes() < 660 && lecture.GetEndInMinutes() > 840) { //lecture in lunchtime, bad luck...
                    noLunch = true;
                    break;
                }
                if(lecture.GetEndInMinutes() + 15 > lunch.GetStartInMinutes() && lecture.GetEndInMinutes() < 840) {
                    lunch.SetStartFromMinutes(lecture.GetEndInMinutes() + 15); //if lecture reaches in lunchbreak
                }
                if(lecture.GetStartInMinutes() - 15 < lunch.GetEndInMinutes() && lecture.GetStartInMinutes() > 660) {
                    lunch.SetEndFromMinutes(lecture.GetStartInMinutes()); //if lecture starts in lunchbreak
                }
            }

            /*
            if(noLunch) {
                continue;
            }
            */
            
            int lunchDuration = lunch.GetEndInMinutes() - lunch.GetStartInMinutes();
            if(lunchDuration >= 45 && !noLunch) {
                lunch.building = 304;
                bool notInserted = true;
                int scheduleLength = student.lectureList.Count;
                for(int i = 0; i < scheduleLength; i++) {
                    if(student.lectureList[i].GetStartInMinutes() > lunch.GetStartInMinutes()) {
                        student.lectureList.Insert(i, lunch);
                        notInserted = false;
                    }
                }
                if(notInserted) {
                    student.lectureList.Add(lunch);
                }
            }
        }
    }
}