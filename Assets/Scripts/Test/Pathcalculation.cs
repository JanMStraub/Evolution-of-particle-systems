using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Pathcalculation {

    private GridGraph _gridGraph = AstarPath.active.data.gridGraph;
    private List<GridNode> _openList;
    private List<GridNode> _closedList;
    
    public Pathcalculation(int width, int height) {
        
    }

    private List<GridNode> FindPath(int startX, int startY, int endX, int endY) {
        

        _openList = new List<GridNode>();
        _closedList = new List<GridNode>();
    }

}
