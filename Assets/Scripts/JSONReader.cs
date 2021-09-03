using UnityEngine;

public partial class JSONReader : MonoBehaviour {

    public TextAsset textJSON;

    public LectureList myLectureList = new LectureList();

    void Awake() => myLectureList = JsonUtility.FromJson<LectureList>(textJSON.text);

}