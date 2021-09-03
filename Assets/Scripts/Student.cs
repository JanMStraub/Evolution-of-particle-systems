using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student {
    private int _id;
    private int _faculty;
    private LectureList _lectureList;

    public Student(int id, int faculty) {
        _id = id;
        _faculty = faculty;
        _lectureList = new LectureList();
    }

    public void addLecture (Lecture lec) {
        _lectureList.lecture.Add(lec);
    }
}