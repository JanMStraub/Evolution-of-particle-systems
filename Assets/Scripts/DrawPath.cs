using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class DrawPath : MonoBehaviour {

    public LineRenderer line;

    public void DrawPathOnFloor(Vector3[] path) {
        line.positionCount = path.Length;
        line.SetPositions(path);
        Debug.Log("DrawPathOnFloor");
    }
}
