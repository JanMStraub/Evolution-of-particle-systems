using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private static GameManager _Instance;

    public GameState State;

    public static event Action<GameState> OnGameStateChanced;

    public static GameManager Instance {
        get {return _Instance;}
    }

    private void Awake() {
        if (_Instance != null && _Instance != this) {
            Destroy(this.gameObject);
            return;
        }

        _Instance = this;
        DontDestroyOnLoad(this.gameObject);

        SceneManager.LoadSceneAsync((int)SceneIndexes.TITLE_SCREEN, LoadSceneMode.Additive);
    }

    // Runs before a scene gets loaded
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadMain() {
        Debug.Log("Loading...");

        GameObject main = GameObject.Instantiate(Resources.Load("Main")) as GameObject;
        GameObject.DontDestroyOnLoad(main); 
    }

    public void LoadGame() {
        SceneManager.UnloadSceneAsync((int)SceneIndexes.TITLE_SCREEN);
        SceneManager.LoadSceneAsync((int)SceneIndexes.TEST, LoadSceneMode.Additive);
    }

    public void UpdateGameState(GameState newState) {
        State = newState;

        switch (newState) {
            case GameState.SpawnAgents:
                Debug.Log("SpawnAgents");
                break;
            case GameState.SetAgentCommute:
                Debug.Log("SetAgentCommute");
                break;
            case GameState.StartNavMeshAgents:
                Debug.Log("StartNavMeshAgents");
                break;
            case GameState.RunSimulation:
                Debug.Log("RunSimulation");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanced?.Invoke(newState);
    }
}

public enum GameState {
    SpawnAgents,
    SetAgentCommute,
    StartNavMeshAgents,
    RunSimulation
}