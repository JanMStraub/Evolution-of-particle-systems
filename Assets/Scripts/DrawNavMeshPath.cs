using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawNavMeshPath {

    private LineRenderer _line;

    public static Vector3[] path = new Vector3[0];

    private void Update() {
        if (path != null && path.Length > 1) {
            _line.positionCount = path.Length;
            for (int i = 0; i < path.Length; i++) {
                _line.SetPosition(i, path[i]);
            }
        }
    }
    

    public void SetLine(LineRenderer line) {
        _line = line;
    }
}