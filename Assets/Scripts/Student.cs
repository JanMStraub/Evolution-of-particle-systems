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

    [SerializeField] private Vector3 _spawnPoint;

    public List<Lecture> lectureList = new List<Lecture>();

    public Student(int id, float size, float speed, Vector3 spawnPoint) {
        _id = id;
        _size = size;
        _speed = speed;
        _spawnPoint = spawnPoint;
        _latestLectureEnding = 0;
    }

    public void setFaculty (int faculty) {
        _faculty = faculty;
    }

    public int getFaculty(){
        return _faculty;
    }

    public int getId() {
        return _id;
    }

    public int getTimetableEnd(){
        return _latestLectureEnding;
    }

    public float getSize () {
        return _size;
    }

    public float getSpeed () {
        return _speed;
    }

    public Vector3 getSpawnPoint () {
        return _spawnPoint;
    }

    public void setSize (float size) {
        _size = size;
    }

    public void setSpeed (float speed) {
        _speed = speed;
    } 

    public void setNextLecture () {
        _lectureIndex++;
    }

    public int getLectureIndex () {
        return _lectureIndex;
    }

    public Lecture getCurrentLecture () {
        return lectureList[_lectureIndex];
    }

    public Lecture getNextLecture () {
        if(_lectureIndex < lectureList.Count)
            return lectureList[_lectureIndex];
        return null;
    }

    public List<GameObject> getDoorsWithinCurrentComplex () {
        return _doorsWithinCurrentComplex;
    }

    public void setDoorsWithinCurrentComplex (GameObject door) {
        _doorsWithinCurrentComplex.Add(door);
    }

    public void setTimetableEnd(int newEnd){
        
        if(_latestLectureEnding > newEnd){
            throw new System.Exception("timetable collision");
        }
        else {
            this._latestLectureEnding = newEnd;
        }
    }
}