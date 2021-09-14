using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lecture {

    public int faculty;
    public int building;
    public int number;
    public string start; // "hh:mm"
    public string end; // "hh:mm"


    public int GetEndInMinutes() {
        string[] splitted = end.Split(':');
        return (int.Parse(splitted[0])*60 + (int.Parse(splitted[1])));
    }

    public int GetStartInMinutes() {
        string[] splitted = start.Split(':');
        return (int.Parse(splitted[0])*60 + (int.Parse(splitted[1])));
    }
}


[System.Serializable]
public class LectureList {

    public List<Lecture> lecture;


    public int Size()
    {
        return lecture.Count;
    }
}