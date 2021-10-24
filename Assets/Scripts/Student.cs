using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Student {

    private int _faculty;
    private int _latestLectureEnding; // in minutes
    private int _lectureIndex = 0;
    public float _speed;

    public int _nextAppointment;
    public List<Lecture> lectureList = new List<Lecture>();


    public Student(float speed) {
        _latestLectureEnding = 0;
        _speed = speed;
    }


    public void SetFaculty(int faculty) {
        _faculty = faculty;
    }


    public void SetTimetableEnd(int newEnd){
        
        if(_latestLectureEnding > newEnd){
            throw new System.Exception("timetable collision");
        }
        else {
            this._latestLectureEnding = newEnd;
        }
    }


    public int GetFaculty() {
        return _faculty;
    }


    public int GetTimetableEnd() {
        return _latestLectureEnding;
    }


    public float GetStudentSpeed() {
        return _speed;
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

        if(_nextAppointment == 0) {
            _nextAppointment = lectureList[0].GetStartInMinutes()-15;
        }

        if(time > _nextAppointment) {
            return 1; //have to go anywhere
        }

        return 0; //still doing something
    }
}