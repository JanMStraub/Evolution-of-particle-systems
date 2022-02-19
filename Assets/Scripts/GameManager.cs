using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private static GameManager _gameManagerInstance;
    private List<AsyncOperation> _scenesLoading = new List<AsyncOperation>();

    public GameState state;
    public static event Action<GameState> onGameStateChanced;


    // Instance for reference during run time
    public static GameManager GameManagerInstance {
        get {return _gameManagerInstance;}
    }


    private void Awake() {

        if (_gameManagerInstance != null && _gameManagerInstance != this) {
            Destroy(this.gameObject);
            return;
        }

        _gameManagerInstance = this;
        DontDestroyOnLoad(this.gameObject);

        SceneManager.LoadSceneAsync((int)SceneIndexes.TITLE_SCREEN, LoadSceneMode.Additive);

        Application.targetFrameRate = 100;
    }


    // Runs before a scene gets loaded
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadMain() {

        Debug.Log("Loading...");

        GameObject main = GameObject.Instantiate(Resources.Load("Main")) as GameObject;
        GameObject.DontDestroyOnLoad(main); 
    }


    public void LoadGame() {

        _scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.TITLE_SCREEN));
        _scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.INF, LoadSceneMode.Additive));

    }
    

    // Used to load the simulation step by step to avoid race conditions
    public void UpdateGameState(GameState newState) {

        state = newState;

        switch (newState) {
            case GameState.StudentInitialisation:
                Debug.Log("StudentInitialisation");
                break;
            case GameState.SetAgentCommute:
                Debug.Log("SetAgentCommute");
                break;
            case GameState.RunSimulation:
                Debug.Log("RunSimulation");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        onGameStateChanced?.Invoke(newState);
    }
}