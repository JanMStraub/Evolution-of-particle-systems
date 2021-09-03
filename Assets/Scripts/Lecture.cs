using System.Collections.Generic;

[System.Serializable]
public class Lecture {
    public int faculty;
    public int building;
    public int number;
    public string start;
    public string end;
}

[System.Serializable]
public class LectureList {
    public List<Lecture> lecture;
}