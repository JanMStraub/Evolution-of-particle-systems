using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class CreatingGrid : MonoBehaviour {

    private Grid[,] _gridArray;
    private float _nodeDiameter;
    private int _gridSizeX, _gridSizeY;

    public Transform player;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public LayerMask unwalkableMask;

	Node[,] grid;

	void Start() {
		_nodeDiameter = nodeRadius*2;
		_gridSizeX = Mathf.RoundToInt(gridWorldSize.x/_nodeDiameter);
		_gridSizeY = Mathf.RoundToInt(gridWorldSize.y/_nodeDiameter);
		CreateGrid();
	}

	void CreateGrid() {
		grid = new Node[_gridSizeX,_gridSizeY];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;

		for (int x = 0; x < _gridSizeX; x ++) {
			for (int y = 0; y < _gridSizeY; y ++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * _nodeDiameter + nodeRadius) + Vector3.forward * (y * _nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint,nodeRadius,unwalkableMask));
				grid[x,y] = new Node(walkable,worldPoint);
			}
		}
	}

	public Node GetNodeFromWorldPoint(Vector3 worldPosition) {
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((_gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((_gridSizeY-1) * percentY);
		return grid[x,y];
	}

	void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y));

	
		if (grid != null) {
            Node playerNode = GetNodeFromWorldPoint(player.position);
			foreach (Node n in grid) {
				Gizmos.color = (n.walkable)?Color.white:Color.red;
                if(playerNode == n)
                    Gizmos.color = Color.green;
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (_nodeDiameter-.1f));
			}
		}
	}
}

public class Node {
	
	public bool walkable;
	public Vector3 worldPosition;
	
	public Node(bool _walkable, Vector3 _worldPos) {
		walkable = _walkable;
		worldPosition = _worldPos;
	}
}