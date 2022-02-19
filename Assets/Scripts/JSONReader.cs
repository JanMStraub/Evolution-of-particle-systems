using UnityEngine;

// Used to get lectures from Json file
public class JSONReader : MonoBehaviour {

    public TextAsset textJSON;
    public LectureList myLectureList = new LectureList();


    void Awake() => myLectureList = JsonUtility.FromJson<LectureList>(textJSON.text);
}