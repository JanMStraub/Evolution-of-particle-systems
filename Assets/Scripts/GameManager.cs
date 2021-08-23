using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private static GameManager _Instance;

    private static Initialisation _GameManager;

    public GameObject loadingScreen;

    public Slider slider;

    public GameState State;
    
    private float _totalSceneProgress;

    public static event Action<GameState> OnGameStateChanced;

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    public static GameManager Instance {
        get {return _Instance;}
    }

    private void Awake() {
        if (_Instance != null && _Instance != this) {
            Destroy(this.gameObject);
            return;
        }

        _GameManager = this;
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
        loadingScreen.gameObject.SetActive(true);
        
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.TITLE_SCREEN));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.TEST, LoadSceneMode.Additive));
        
        GameManager.Instance.UpdateGameState(GameState.SpawnAgents);
        Debug.Log("GameState updated");

        StartCoroutine(GetSceneLoadProcess());
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


    IEnumerator GetSceneLoadProcess () {
        for (int i = 0; i < scenesLoading.Count; i++) {
            while (!scenesLoading[i].isDone) {
                _totalSceneProgress = 0;

                foreach (AsyncOperation operation in scenesLoading) {
                    _totalSceneProgress += operation.progress;
                }

                _totalSceneProgress = (_totalSceneProgress / scenesLoading.Count) * 100f;

                slider.value = Mathf.RoundToInt(_totalSceneProgress);

                yield return null;
            }
        }
        loadingScreen.gameObject.SetActive(false);
    }

}

public enum SceneIndexes {
    MANAGER = 0,
    TITLE_SCREEN = 1,
    TEST = 2
}

public enum GameState {
    SpawnAgents,
    SetAgentCommute,
    StartNavMeshAgents,
    RunSimulation
}