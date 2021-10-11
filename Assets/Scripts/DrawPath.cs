using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class DrawPath : MonoBehaviour {

    private static DrawPath _drawPathInstance;
    private List<Tupel> _lineRendererAndPathTupel = new List<Tupel>(); 

    public LineRenderer line;

    public static DrawPath DrawPathInstance {
        get {return _drawPathInstance;}
    }


    private void Awake() {
        _drawPathInstance = this;
    }


    private void Update() {
        foreach (Tupel tupel in _lineRendererAndPathTupel) {
            tupel.Item1.positionCount = tupel.Item2.Length;
            tupel.Item1.SetPositions(path);
        }
    }


    public void AddPathToList(LineRenderer line, Vector3[] path) {
        /*
        line.positionCount = path.Length;
        line.SetPositions(path);
        _lineRendererList.Add(line);
        */
        var tupel = new Tupel<List<LineRenderer>, Vector3[]>(line, path);
        _lineRendererAndPathTupel.Add(tupel);
    }
}
