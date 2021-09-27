using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Student {

    private int _id;
    private int _faculty;
    private int _latestLectureEnding; // in minutes
    private int _nextLecture;
    private float _size;
    private float _speed;
    private int _lectureIndex = 0;
    private List<GameObject> _doorsWithinCurrentComplex = new List<GameObject>();
    private bool _currentlyEnRoute = false;
    private bool _dayFinished = false;

    public int _nextAppointment;

    [SerializeField] private Vector3 _spawnPoint;

    public List<Lecture> lectureList = new List<Lecture>();


    public Student(int id, float size, float speed, Vector3 spawnPoint) {
        _id = id;
        _size = size;
        _speed = speed;
        _spawnPoint = spawnPoint;
        _latestLectureEnding = 0;

    }


    public void SetFaculty(int faculty) {
        _faculty = faculty;
    }


    public void SetDoorsWithinCurrentComplex(GameObject door) {
        _doorsWithinCurrentComplex.Add(door);
    }


    public void SetTimetableEnd(int newEnd){
        
        if(_latestLectureEnding > newEnd){
            throw new System.Exception("timetable collision");
        }
        else {
            this._latestLectureEnding = newEnd;
        }
    }


    public void SetSize(float size) {
        _size = size;
    }


    public void SetSpeed(float speed) {
        _speed = speed;
    } 


    public void SetNextLecture() {
        _lectureIndex++;
    }


    public void SetCurrentlyEnRoute(bool currentlyEnRoute) {
        _currentlyEnRoute = currentlyEnRoute;
    }


    public void SetDayFinished() {
        _dayFinished = true;
    }


    public int GetFaculty(){
        return _faculty;
    }


    public int GetId() {
        return _id;
    }


    public int GetTimetableEnd(){
        return _latestLectureEnding;
    }


    public float GetSize() {
        return _size;
    }


    public float GetSpeed() {
        return _speed;
    }


    public Vector3 GetSpawnPoint() {
        return _spawnPoint;
    }


    public int GetLectureIndex() {
        return _lectureIndex;
    }


    // Exception ?
    public Lecture GetCurrentLecture() {
        if (lectureList.Count != 0)
            return lectureList[_lectureIndex];
        else
            throw new System.Exception("hab ich dich du Schlingel");
    }


    public Lecture GetNextLecture() {
        if(_lectureIndex + 1 < lectureList.Count) {
            return lectureList[_lectureIndex + 1];
        } else if(_lectureIndex < lectureList.Count) {
            return lectureList[_lectureIndex];
        } else {
            Debug.Log("this is silly");
            return null;
        }
    }


    public bool GetCurrentlyEnRoute() {
        return _currentlyEnRoute;
    }


    public bool GetDayFinished() {
        return _dayFinished;
    }


    public List<GameObject> GetDoorsWithinCurrentComplex() {
        return _doorsWithinCurrentComplex;
    }


    public void EmptyCurrentLectureDoorList () {
        _doorsWithinCurrentComplex.Clear();
    }


    public string[] RoutePoints() {
        string actualPosition = "";
        string nextPosition = "";
        if(_lectureIndex == 0) { //first lecture, start from spawnpoint
            actualPosition = "Spawn";
            nextPosition = "" + lectureList[_lectureIndex].building;
        } else if(_lectureIndex == lectureList.Count) { //last lecture, go back to spawnpoint
            actualPosition = "" + lectureList[_lectureIndex-1].building;
            nextPosition = "Spawn";
            _lectureIndex = - 2; //day over
        } else {
            actualPosition = "" + lectureList[_lectureIndex-1].building;
            nextPosition = "" + lectureList[_lectureIndex].building;
        }

        _lectureIndex++;

        if(_lectureIndex >= lectureList.Count) {
            _nextAppointment = lectureList[_lectureIndex-1].GetEndInMinutes() + 5; //the bell does not dismiss you, i do
        } else if(_lectureIndex > 0) {
            _nextAppointment = lectureList[_lectureIndex].GetStartInMinutes() - 15; //but sit on your place when lecture starts
        }

        return new string[]{actualPosition, nextPosition};
    }

    public int check(int time) {
        if(_lectureIndex < 0) {
            return 2; //day over, no more checks necessary
        }

        if(time > _nextAppointment) {
            return 1; //have to go anywhere
        }

        return 0; //still doing something
    }
}