using UnityEngine;
using System.Collections;

public enum SceneIndexes {
    MANAGER = 0,
    TITLE_SCREEN = 1,
    INF = 2
}

public enum GameState {
    SpawnAgents,
    SetAgentCommute,
    StartNavMeshAgents,
    RunSimulation
}