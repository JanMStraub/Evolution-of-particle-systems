using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using CodeMonkey.Utils;

public class UnitGameObject : MonoBehaviour {

    [SerializeField] private ConvertedEntityHolder convertedEntityHolder;

    private void Start() {
        Debug.Log(convertedEntityHolder.GetEntity());
    }

    private void Update() {
        Entity entity = convertedEntityHolder.GetEntity();
        EntityManager entityManager = convertedEntityHolder.GetEntityManager();

        if (Input.GetMouseButtonDown(0)) {
            // Give Unit Move Order
	        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();

	        float cellSize = PathfindingGridSetup.Instance.pathfindingGrid.GetCellSize();

	        PathfindingGridSetup.Instance.pathfindingGrid.GetXY(mousePosition + new Vector3(1, 1) * cellSize * +.5f, out int endX, out int endY);
	
	        ValidateGridPosition(ref endX, ref endY);

		    PathfindingGridSetup.Instance.pathfindingGrid.GetXY(transform.position + new Vector3(1, 1, 0) * cellSize * +.5f, out int startX, out int startY);

		    ValidateGridPosition(ref startX, ref startY);

            entityManager.AddComponentData(entity, 
                new PathfindingParams { 
			    startPosition = new int2(startX, startY), 
                endPosition = new int2(endX, endY) 
            });
        }

        // Follow the Path
        PathFollow pathFollow = entityManager.GetComponentData<PathFollow>(entity);
        DynamicBuffer<PathPosition> pathPositionBuffer = entityManager.GetBuffer<PathPosition>(entity);

        if (pathFollow.pathIndex >= 0) {
            // Has path to follow
            PathPosition pathPosition = pathPositionBuffer[pathFollow.pathIndex];

            float3 targetPosition = new float3(pathPosition.position.x, pathPosition.position.y, 0);
            float3 moveDir = math.normalizesafe(targetPosition - (float3)transform.position);
            float moveSpeed = 3f;

            transform.position += (Vector3)(moveDir * moveSpeed * Time.deltaTime);

            if (math.distance(transform.position, targetPosition) < .1f) {
                // Next waypoint
                pathFollow.pathIndex--;
                entityManager.SetComponentData(entity, pathFollow);
            }
        } else {
            //*
            // Find new random move position
            int mapWidth = PathfindingGridSetup.Instance.pathfindingGrid.GetWidth();
            int mapHeight = PathfindingGridSetup.Instance.pathfindingGrid.GetHeight();
            float3 originPosition = float3.zero;
            float cellSize = PathfindingGridSetup.Instance.pathfindingGrid.GetCellSize();
            Unity.Mathematics.Random random = new Unity.Mathematics.Random((uint)UnityEngine.Random.Range(0, 10000));

            GetXY(transform.position + new Vector3(1, 1, 0) * cellSize * +.5f, originPosition, cellSize, out int startX, out int startY);

            ValidateGridPosition(ref startX, ref startY);

            int endX = random.NextInt(0, mapWidth);
            int endY = random.NextInt(0, mapHeight);
            
            entityManager.AddComponentData(entity, 
                new PathfindingParams { 
			    startPosition = new int2(startX, startY), 
                endPosition = new int2(endX, endY) 
            });
            //*/
        }
    }

    private void ValidateGridPosition(ref int x, ref int y) {
        x = math.clamp(x, 0, PathfindingGridSetup.Instance.pathfindingGrid.GetWidth() - 1);
        y = math.clamp(y, 0, PathfindingGridSetup.Instance.pathfindingGrid.GetHeight() - 1);
    }

    private static void GetXY(float3 worldPosition, float3 originPosition, float cellSize, out int x, out int y) {
        x = (int)math.floor((worldPosition - originPosition).x / cellSize);
        y = (int)math.floor((worldPosition - originPosition).y / cellSize);
    }

}
