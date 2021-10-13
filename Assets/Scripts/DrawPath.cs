using UnityEngine;

class DrawPath : MonoBehaviour {

    private float _width = 0.2f;

    public LineRenderer line;

    public void DrawPathOnFloor(Vector3[] path) {
        this.line.positionCount = path.Length;
        this.line.SetPositions(path);
    }

    public void ChangeWidthOfLine() {
        this.line.widthMultiplier = _width;
        if (_width < 10f) {
            _width = _width + 0.05f;
        }
    }
}