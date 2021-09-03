using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student {
    private double _id;
    private int _faculty;
    public List<Lecture> lectureList = new List<Lecture>();

    public Student (double id) {
        _id = id;
    }

    public void setFaculty (int faculty) {
        _faculty = faculty;
    }

    // TODO Student size 
    // TODO Student speed
}