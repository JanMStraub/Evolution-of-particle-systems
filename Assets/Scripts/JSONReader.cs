using UnityEngine;
 
 public class JSONReader : MonoBehaviour {

    public TextAsset textJSON;

    [System.Serializable]
    public class Lecture {
        public int building;
        public int number;
        public string start;
        public string end;
    }

    [System.Serializable]
    public class LectureList {
        public Lecture[] lecture;
    }

    public LectureList myLectureList = new LectureList();

    void Start () {
        myLectureList = JsonUtility.FromJson<LectureList>(textJSON.text);
    }

}