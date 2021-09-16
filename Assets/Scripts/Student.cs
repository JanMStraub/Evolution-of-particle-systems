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


    public Lecture GetCurrentLecture() {
        return lectureList[_lectureIndex];
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
}