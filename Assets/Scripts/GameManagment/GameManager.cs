using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Slider slider;
    public GameState state;
    public static event Action<GameState> onGameStateChanced;
    public GameObject loadingScreen;

    private static GameManager _gameManagerInstance;
    private List<AsyncOperation> _scenesLoading = new List<AsyncOperation>();
    private float _totalSceneProgress;
    // private float _totalSpawnProgress = 0;
    // private float _totalCommuteProgress = 0;
    

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


    // Managing LoadingScreen
    IEnumerator GetSceneLoadProcess() {
        
        for (int i = 0; i < _scenesLoading.Count; i++) {
            while (!_scenesLoading[i].isDone) {
                _totalSceneProgress = 0;

                foreach (AsyncOperation operation in _scenesLoading) {
                    _totalSceneProgress += operation.progress;
                }

                _totalSceneProgress = (_totalSceneProgress / _scenesLoading.Count) * 100f;

                yield return null;
            }
        }
    }


    /*
    IEnumerator GetSpawnProgress() {
        while (SpawnController.SpawnControllerInstance == null || !SpawnController.SpawnControllerInstance.isDone) {
            if (SpawnController.SpawnControllerInstance != null) {
                _totalSpawnProgress = Mathf.Round(SpawnController.SpawnControllerInstance.spawnProgress * 100f);
            }
            yield return null;
        }
    }


    IEnumerator GetTotalProgress() {
        float totalProgress = 0;

        while (CommuteController.CommuteControllerInstance == null || !CommuteController.CommuteControllerInstance.isDone) {
            if (CommuteController.CommuteControllerInstance != null) {
                _totalCommuteProgress = Mathf.Round(CommuteController.CommuteControllerInstance.commuteProgress * 100f);
            }

            totalProgress = Mathf.Round((_totalSceneProgress + _totalSpawnProgress + _totalCommuteProgress) / 3f);
            slider.value = Mathf.RoundToInt(totalProgress);

            yield return null;
        }
        
        loadingScreen.gameObject.SetActive(false);
    }
    */
}
