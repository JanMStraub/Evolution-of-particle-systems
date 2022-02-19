using System.Collections.Generic;

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


    // Set start time as HH:MM from minutes
    public void SetStartFromMinutes(int minutes) {
        string startString = "";
        if((int)(minutes/60f) < 10) {
            startString += "0";
        }
        startString += (int)(minutes/60f);
        startString += ":";

        if((int)(minutes%60f) < 10) {
            startString += "0";
        }
        startString += (int)(minutes%60f);
        this.start = startString;
    }


    // Set endtime as HH:MM from minutes
    public void SetEndFromMinutes(int minutes) {
        string endString = "";
        if((int)(minutes/60f) < 10) {
            endString += "0";
        }
        endString += (int)(minutes/60f);
        endString += ":";
        
        if((int)(minutes%60f) < 10) {
            endString += "0";
        }
        endString += (int)(minutes%60f);
        this.end = endString;
    }
}


[System.Serializable]
public class LectureList {

    public List<Lecture> lecture;


    public int Size() {
        return lecture.Count;
    }
}