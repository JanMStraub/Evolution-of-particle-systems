using UnityEngine;

class DrawPath : MonoBehaviour {

    public LineRenderer line;

    public void DrawPathOnFloor(Vector3[] path) {
        line.positionCount = path.Length;
        line.SetPositions(path);
    }
}