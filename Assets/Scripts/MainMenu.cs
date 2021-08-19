using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class MainMenu : MonoBehaviour {

    public void StartSimulation () {
        SceneManager.UnloadSceneAsync((int)SceneIndexes.TITLE_SCREEN);
        SceneManager.LoadSceneAsync((int)SceneIndexes.TEST, LoadSceneMode.Additive);
        GameManager.Instance.UpdateGameState(GameState.SpawnAgents);
        Debug.Log("GameState updated");
    }

    public void QuitSimulation () {
        Application.Quit();
    }
}
