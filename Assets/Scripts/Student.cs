using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Student {
    private int _id;

    private int _faculty;

    private int _latestLectureEnding; // in minutes

    private float _size;

    private float _speed;

    public List<Lecture> lectureList = new List<Lecture>();

    public Student(int id, float size, float speed) {
        _id = id;
        _size = size;
        _speed = speed;
        _latestLectureEnding = 0;
    }

    public void setFaculty (int faculty) {
        _faculty = faculty;
    }

    public int getFaculty(){
        return _faculty;
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

    public void setSize (float size) {
        _size = size;
    }

    public void setSpeed (float speed) {
        _speed = speed;
    } 

    public void setTimetableEnd(int newEnd){
        
        if(_latestLectureEnding > newEnd){
            throw new System.Exception("timetable collision");
        }
        else {
            this._latestLectureEnding = newEnd;
        }
        
        this._latestLectureEnding = newEnd;
    }
}