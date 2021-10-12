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
    public GameObject loadingScreen;


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
        
        _scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.TITLE_SCREEN));
        _scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.INF, LoadSceneMode.Additive));

        loadingScreen.gameObject.SetActive(false);
    }
    

    // Managing GameStates 
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