using UnityEngine;

public class JSONReader : MonoBehaviour {

    public TextAsset textJSON;
    public LectureList myLectureList = new LectureList();


    void Awake() => myLectureList = JsonUtility.FromJson<LectureList>(textJSON.text);
}