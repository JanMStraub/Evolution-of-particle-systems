using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Student {
    private double _id;
    private int _faculty;
    private int _latestLectureEnding; // in minutes
    public List<Lecture> lectureList = new List<Lecture>();

    public Student (double id) {
        _id = id;
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

    public void setTimetableEnd(int newEnd){
        /*
        if(_latestLectureEnding > newEnd){
            throw new System.Exception("timetable collision");
        }
        else{
            this._latestLectureEnding = newEnd;
        }
        */
        this._latestLectureEnding = newEnd;
    }

    // TODO Student size 
    // TODO Student speed
}